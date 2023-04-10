using System.Net;

namespace conan_master_server.Additional;

public class ServerIpMiddleware
{
    private const string SMIP = "91.233.169.34";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly ILogger<ServerIpMiddleware> _logger;

    public ServerIpMiddleware(RequestDelegate next, IConfiguration config, ILogger<ServerIpMiddleware> logger)
    {
        _next = next;
        _config = config;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress.ToString();

        var localIp = _config.GetSection("LocalIp").Value;
        _logger.LogInformation(
            $"ServerIpMiddleware. RemoteIP: {remoteIp} | LocalIP: {localIp} | Equal: {remoteIp == localIp}");
        if (remoteIp == localIp)
            context.Connection.RemoteIpAddress = IPAddress.Parse(SMIP);

        await _next(context);
    }
}

public static class ServerIpMiddlewareExtensions
{
    public static IApplicationBuilder UseServerIpMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ServerIpMiddleware>();
}