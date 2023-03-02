using conan_master_server.Data;

namespace conan_master_server.ServerLogic;

public class ServerCleaner
{
    public IServiceScope Scope { get; set; }

    public async Task Cleanup()
    {
        var db = Scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await db.Database.EnsureCreatedAsync();
        await db.SaveChangesAsync();

        while (true)
        {
            var cutoff = DateTime.Now.AddMinutes(-2);
            var serversToDelete = db.Servers.Where(s => s.LastPing < cutoff).ToList();
            db.Servers.RemoveRange(serversToDelete);
            await db.SaveChangesAsync();
            await Task.Delay(TimeSpan.FromMinutes(2));
        }
    }
}