using conan_master_server.Data;
using conan_master_server.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace conan_master_server.ServerLogic;

public class ServerHandler
{
    private readonly CleanerOrchestrator _cleanerOrchestrator;
    private readonly ILogger<ServerHandler> _logger;
    private readonly IConfiguration _config;

    public ServerHandler(CleanerOrchestrator cleanerOrchestrator, ILogger<ServerHandler> logger, IConfiguration config)
    {
        _cleanerOrchestrator = cleanerOrchestrator;
        _logger = logger;
        _config = config;
    }
    
    public async Task InitialHandler(string message, string remoteIp, DatabaseContext db)
    {
        ServerEntity server;
        try
        {
            var jObj = JObject.Parse(message);

            var sdObj = jObj["sessionDoc"] as JObject;
            var id = remoteIp + ":" + sdObj["Port"];
            
            _logger.LogInformation($"Connection: {id}");
            
            if (sdObj["ip"].Contains(_config.GetSection("LocalIp").Value))
            {
                sdObj["CSF"] = 1;
                sdObj["Name"] = "FreeTP#1";
            }
            
            sdObj.Add("id", id);

            server = JsonConvert.DeserializeObject<ServerEntity>(sdObj.ToString())!;
        }
        catch (Exception e)
        {
            _logger.LogError("Probably no suitable entity for deserialization. Exact error: "+e.Message);
            return;
        }
        
        server.ip = remoteIp;
        server.LastPing = DateTime.Now;
        
        var s = db.Servers.FirstOrDefault(x => x.Id == server.Id);
        if (s != null && !server.Equals(s))
        {
            db.Entry(s).CurrentValues.SetValues(server);
        }
        else
        {
            db.Servers.Add(server);
        }

        if (s != null && db.Entry(s).State == EntityState.Unchanged)
            return;
        
        await db.SaveChangesAsync();
        _cleanerOrchestrator.Run();
    }
}