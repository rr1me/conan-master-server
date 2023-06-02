using conan_master_server.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace conan_master_server.Additional;

public class RequestHandler
{
    private readonly IHttpClientFactory _clientFactory;

    public RequestHandler(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    private const string KEY = "0C26A951282D879B22CEC44F9B87F546";
    
    private const string STEAM_TICKETAUTH_URL = "https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v0001/";
    private const string STEAM_GETUSER_URL = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/";

    public async Task<Params> AuthUserTicket(string steamTicket)
    {
        var param = new Dictionary<string, string>
        {
            {"appid", "480"},
            {"key", KEY},
            {"ticket", steamTicket}
        };
        return await MakeRequest<Params>(STEAM_TICKETAUTH_URL, param);
    }

    public async Task<SteamPlayerInfo> GetUserInfo(long steamId)
    {
        var param = new Dictionary<string, string>
        {
            { "key", KEY },
            { "steamids", steamId.ToString() }
        };

        return await MakeRequest<SteamPlayerInfo>(STEAM_GETUSER_URL, param);
    }

    private async Task<T> MakeRequest<T>(string url, IDictionary<string, string> param)
    {
        var uri = new Uri(QueryHelpers.AddQueryString(url, param));
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var httpClient = _clientFactory.CreateClient();
        var r = await httpClient.SendAsync(request);
        var responseString = await r.Content.ReadAsStringAsync();

        var jToken = JObject.Parse(responseString)["response"].First.First;

        if (jToken.Count() == 2)
            throw new ArgumentException("Invalid steam ticket");
        
        var json = jToken.Type == JTokenType.Array ? jToken.First.ToString() : jToken.ToString();

        return JsonConvert.DeserializeObject<T>(json);
    }
}