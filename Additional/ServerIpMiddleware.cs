using System.Net;

namespace conan_master_server.Additional;

public class ServerIpMiddleware
{
    private const string SMIP = "91.233.169.34";

    private readonly RequestDelegate _next;

    public ServerIpMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress.ToString();

        if (remoteIp is "192.168.0.202" or "192.168.0.201")
            context.Connection.RemoteIpAddress = IPAddress.Parse(SMIP);

        await _next(context);
    }
}

public static class ServerIpMiddlewareExtensions
{
    public static IApplicationBuilder UseServerIpMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ServerIpMiddleware>();
}