using conan_master_server.Data;
using conan_master_server.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace conan_master_server.ServerLogic;

public class ServerHandler
{
    public async Task InitialHandler(string message, DatabaseContext db)
    {
        ServerEntity server;
        try
        {
            var jObj = JObject.Parse(message);

            var id = jObj["serverAddress"];
            var sdObj = jObj["sessionDoc"] as JObject;
            sdObj.Add("id", id);

            server = JsonConvert.DeserializeObject<ServerEntity>(sdObj.ToString());
        }
        catch (Exception e)
        {
            throw new JsonException("Probably no suitable entity for deserialization. Exact error: "+e.Message);
        }
        
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
        
        if (s == null || db.Entry(s).State != EntityState.Unchanged)
            await db.SaveChangesAsync();
    }
}