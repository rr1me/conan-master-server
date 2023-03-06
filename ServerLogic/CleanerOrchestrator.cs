namespace conan_master_server.ServerLogic;

public class CleanerOrchestrator
{
    private readonly ILogger<CleanerOrchestrator> _logger;
    private readonly IServiceProvider _provider;
    private Task _serverCleaner;

    public CleanerOrchestrator(IServiceProvider provider, ILogger<CleanerOrchestrator> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("Trying to run cleaner task. Status: " + (_serverCleaner is not null ? _serverCleaner.Status : "Task wasn't created"));
        if (_serverCleaner is { IsCompleted: false }) return;

        var scope = _provider.CreateScope().ServiceProvider;
        var cleaner = scope.GetRequiredService<ServerCleaner>();
        _serverCleaner = Task.Run(cleaner.Cleanup);
    }
}