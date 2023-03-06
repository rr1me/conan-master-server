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

    public ServerHandler(CleanerOrchestrator cleanerOrchestrator, ILogger<ServerHandler> logger)
    {
        _cleanerOrchestrator = cleanerOrchestrator;
        _logger = logger;
    }

    private const string smip = "91.233.169.34";
    
    public async Task InitialHandler(string message, string remoteIp, DatabaseContext db)
    {
        ServerEntity server;
        var ip = remoteIp is "192.168.0.202" or "192.168.0.201" ? smip : remoteIp;
        try
        {
            var jObj = JObject.Parse(message);

            var sdObj = jObj["sessionDoc"] as JObject;
            var id = ip + ":" + sdObj["Port"];
            
            _logger.LogInformation($"{ip} | {remoteIp} | {id}");
            
            sdObj.Add("id", id);

            server = JsonConvert.DeserializeObject<ServerEntity>(sdObj.ToString())!;
        }
        catch (Exception e)
        {
            throw new JsonException("Probably no suitable entity for deserialization. Exact error: "+e.Message);
        }
        
        server.ip = ip;
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