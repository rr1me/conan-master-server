using System.Net.WebSockets;
using System.Text;

namespace conan_master_server.ServerLogic;

public class SocketHandler
{
    public async Task Handle(HttpContext httpContext, Func<string, string, Task> callback)
    {
        if (httpContext.WebSockets.IsWebSocketRequest)
        {
            await Echo(httpContext, callback);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    
    private async Task Echo(HttpContext httpContext, Func<string, string, Task> callback)
    {
        using var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
        
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!receiveResult.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            var remoteIp = httpContext.Request.Headers["X-Forwarded-For"];
            await callback(message, string.IsNullOrWhiteSpace(remoteIp.ToString())? remoteIp : httpContext.Connection.RemoteIpAddress.ToString());

            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            try
            {
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            catch (WebSocketException e)
            {
                Console.WriteLine("Probably the remote party closed ws connection like -9. Exact error: " + e.Message);
            }
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}