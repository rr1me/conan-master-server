using System.Net.WebSockets;
using System.Text;

namespace conan_master_server.ServerLogic;

public class SocketHandler
{
    public async Task Handle(HttpContext httpContext, Func<string, Task> callback)
    {
        if (httpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket, callback);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    
    private async Task Echo(WebSocket webSocket, Func<string, Task> callback)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!receiveResult.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            await callback(message);
            
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}