using conan_master_server.Data;

namespace conan_master_server.ServerLogic;

public class ServerCleaner
{
    private readonly DatabaseContext _db;

    public ServerCleaner(DatabaseContext db)
    {
        _db = db;
    }

    public async Task Cleanup()
    {
        while (true)
        {
            try
            {
                if (!_db.Servers.Any()) break;
                
                var cutoff = DateTime.Now.AddMinutes(-2);
                var serversToDelete = _db.Servers.Where(s => s.LastPing < cutoff).ToList();
                _db.Servers.RemoveRange(serversToDelete);
                await _db.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromSeconds(15));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                break;
            }
        }
    }
}