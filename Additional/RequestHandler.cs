using conan_master_server.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace conan_master_server.Additional;

public class RequestHandler
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string KEY;

    public RequestHandler(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _clientFactory = clientFactory;
        KEY = config.GetSection("Key").Value;
    }
    
    private const string STEAM_TICKETAUTH_URL = "https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v0001/";
    private const string STEAM_GETUSER_URL = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/";

    private const string FANCOM_API_URL = "https://exiles-fls-live.azurewebsites.net/api/VerifyIdentity?code=OOsFrduVe5Ph8HLvZB58YDSvv20WTHzYMaeH9dCB7HBM7TYpedALuA==";
    
    public async Task<Params> AuthUserTicket(string steamTicket)
    {
        var param = new Dictionary<string, string>
        {
            { "appid", "480" },
            { "key", KEY },
            { "ticket", steamTicket }
        };
        return await MakeSteamRequest<Params>(STEAM_TICKETAUTH_URL, param);
    }

    public async Task<SteamPlayerInfo> GetUserInfo(long steamId)
    {
        var param = new Dictionary<string, string>
        {
            { "key", KEY },
            { "steamids", steamId.ToString() }
        };

        return await MakeSteamRequest<SteamPlayerInfo>(STEAM_GETUSER_URL, param);
    }

    private async Task<T> MakeSteamRequest<T>(string url, IDictionary<string, string> param)
    {
        var uri = new Uri(QueryHelpers.AddQueryString(url, param));
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var responseString = await ProcessRequest(request);

        var jToken = JObject.Parse(responseString)["response"].First.First;

        if (jToken.Count() == 2)
            throw new ArgumentException("Invalid steam ticket");
        
        var json = jToken.Type == JTokenType.Array ? jToken.First.ToString() : jToken.ToString();

        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<string> MakeFancomRequest(object obj)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, FANCOM_API_URL);
        request.Content = new StringContent(JsonConvert.SerializeObject(obj));
        return await ProcessRequest(request);
    }

    private async Task<string> ProcessRequest(HttpRequestMessage request)
    {
        var httpClient = _clientFactory.CreateClient();
        var r = await httpClient.SendAsync(request);
        return await r.Content.ReadAsStringAsync();
    }
}