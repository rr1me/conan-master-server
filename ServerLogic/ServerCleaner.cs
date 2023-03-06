using conan_master_server.Data;

namespace conan_master_server.ServerLogic;

public class ServerCleaner
{
    private readonly DatabaseContext _db;
    private readonly ILogger<ServerCleaner> _logger;

    public ServerCleaner(DatabaseContext db, ILogger<ServerCleaner> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Cleanup()
    {
        while (true)
        {
            try
            {
                _logger.LogInformation("Clearing task iteration started");
                if (!_db.Servers.Any())
                {
                    _logger.LogInformation("Finishing clearing task due to empty servers db");
                    break;
                }

                var cutoff = DateTime.Now.AddMinutes(-2);
                var serversToDelete = _db.Servers.Where(s => s.LastPing < cutoff).ToList();
                _db.Servers.RemoveRange(serversToDelete);
                var changes = await _db.SaveChangesAsync();
                
                _logger.LogInformation($"Clearing task iteration finished. Entities to delete: {changes}" +
                                       (changes > 0 ? 
                                           ". Ids: " + string.Join(" | ", serversToDelete.Select(x => x.Id))
                                           : null));
                
                await Task.Delay(TimeSpan.FromMinutes(2));
            }
            catch (Exception e)
            {
                _logger.LogWarning("Task exception. Error message: " + e.Message);
                break;
            }
        }
    }
}