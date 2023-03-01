using conan_master_server.Data;

namespace conan_master_server.ServerLogic;

public class ServerCleaner
{
    public DatabaseContext _db { get; set; }

    public async Task Cleanup()
    {
        while (true)
        {
            var cutoff = DateTime.Now.AddMinutes(2);
            var serversToDelete = _db.Servers.Where(s => s.LastPing < cutoff).ToList();
            _db.Servers.RemoveRange(serversToDelete);
            await _db.SaveChangesAsync();
            await Task.Delay(TimeSpan.FromMinutes(2));
        }
    }
}