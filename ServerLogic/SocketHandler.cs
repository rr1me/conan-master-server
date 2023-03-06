using System.Net.WebSockets;
using System.Text;

namespace conan_master_server.ServerLogic;

public class SocketHandler
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<SocketHandler> _logger;

    public SocketHandler(IHostApplicationLifetime lifetime, ILogger<SocketHandler> logger)
    {
        _lifetime = lifetime;
        _logger = logger;
    }

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
        WebSocketReceiveResult receiveResult = null;
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), _lifetime.ApplicationStopping);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Probably the remote party closed ws connection like -9 or application shutting down. Exact error: " + e.Message);
                return;
            }

            if (receiveResult.MessageType == WebSocketMessageType.Close)
            {
                _logger.LogInformation("Closing ws connection due to wsMessageType == Close");
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            
            await callback(message, httpContext.Connection.RemoteIpAddress.ToString());

            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                _lifetime.ApplicationStopping);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            _lifetime.ApplicationStopping);
    }
}