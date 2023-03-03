namespace conan_master_server.ServerLogic;

public class CleanerOrchestrator
{
    private readonly IServiceProvider _provider;
    private Task _serverCleaner;

    public CleanerOrchestrator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void Run()
    {
        if (_serverCleaner is { IsCompleted: false })
            throw new ApplicationException("Unable to run new task. Task status: " + _serverCleaner.Status);

        var scope = _provider.CreateScope().ServiceProvider;
        var cleaner = scope.GetRequiredService<ServerCleaner>();
        _serverCleaner = Task.Run(cleaner.Cleanup);
    }
}