using conan_master_server.Data;
using conan_master_server.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace conan_master_server.ServerLogic;

public class ServerHandler
{
    public DatabaseContext _db { get; set; }

    public async Task InitialHandler(string message)
    {
        var jObj = JObject.Parse(message);

        var id = jObj["serverAddress"];
        var sdObj = jObj["sessionDoc"] as JObject;
        sdObj.Add("id", id);
        
        var server = JsonConvert.DeserializeObject<ServerEntity>(sdObj.ToString());
        server.LastPing = DateTime.Now;
        Console.WriteLine(server.LastPing);
        
        var s = _db.Servers.FirstOrDefault(x => x.Id == server.Id);
        if (s != null && !server.Equals(s))
        {
            _db.Entry(s).CurrentValues.SetValues(server);
        }
        else
        {
            _db.Servers.Add(server);
        }
        
        if (s == null || _db.Entry(s).State != EntityState.Unchanged)
            await _db.SaveChangesAsync();
    }
}