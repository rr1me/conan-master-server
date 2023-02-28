using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.Models;
using conan_master_server.Tickets;
using conan_master_server.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace conan_master_server.Controllers;

public class ConanController : ControllerBase
{
    private readonly PlayerData _playerData;
    private readonly RequestHandler _requestHandler;
    private readonly DatabaseContext _db;
    private readonly RandomGenerator _randomGenerator;
    private readonly ResponseWrapper _wrapper;
    private readonly TokenGenerator _tokenGenerator;

    public ConanController(PlayerData playerData, RequestHandler requestHandler, DatabaseContext db,
        RandomGenerator randomGenerator, ResponseWrapper wrapper, TokenGenerator tokenGenerator)
    {
        _playerData = playerData;
        _requestHandler = requestHandler;
        _db = db;
        _randomGenerator = randomGenerator;
        _wrapper = wrapper;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthTicket authTicket)
    {
        var steamResponse = await _requestHandler.AuthUserTicket(authTicket.SteamTicket);

        _wrapper.data = await _playerData.Generate(steamResponse.SteamId, authTicket.TitleId, _db, HttpContext,
            _randomGenerator);
        return Ok(_wrapper);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetIdentityToken([FromBody] TokenRequest tokenRequest)
    {
        _wrapper.data = new
        {
            FunctionResult = _tokenGenerator.Generate()
        };
        return Ok(_wrapper);
    }

    [HttpPost("/cloud")]
    public async Task<IActionResult> CloudScript()
    {
        _wrapper.data = new
        {
            FunctionResult = new
            {
                IsPlayerBanned = false,
                ChargebackAdjustedVirtualCurrencies = new { },
                BannedNegativeVirtualCurrencies = new { }
            }
        };

        return Ok(_wrapper);
    }

    [HttpGet("/servers")]
    public async Task<IActionResult> GetServers(Server server)
    {
        var s = _db.Servers.FirstOrDefault(x => x.Ip == server.Ip);
        if (s != null && !server.Equals(s))
        {
            _db.Servers.Update(server);
        }
        else
        {
            _db.Servers.Add(server);
        }
        
        
        var r = new
        {
            sessions = new List<dynamic>
            {
                new
                {
                    S9 = "6_6_6",
                    S8 = 0.0,
                    S24 = 0.0,
                    S119 = false,
                    S117 = false,
                    ip = server.Ip, //
                    S0 = server.Pvp, //
                    S7 = false,
                    S6 = 0.0,
                    Private = false,
                    S4 = 0.0,
                    S15 = 0,
                    S18 = false,
                    Sl = 0,
                    buildId = 1654935032,
                    S17 = "",
                    S30 = 0,
                    maxplayers = server.MaxPlayers, //
                    kdsUri = "https://ce-kds-winunoff-ams05.funcom.com:7001/",
                    Sz = 0,
                    Sy = 0,
                    serverUID = "436DD9864AC50173011009877349A33A",
                    Name = server.Name, //
                    S122 = false,
                    S120 = "123",
                    CSF = 1,
                    Sw = 0,
                    Su = 0,
                    S22 = 0,
                    S23 = 0,
                    S21 = 0,
                    So = 0,
                    Sm = 0,
                    S25 = false,
                    Sa = false,
                    Sg = 0,
                    Sf = 0,
                    MapName = server.MapName, //
                    displayedmaxplayers = 10,
                    Sj = 0,
                    ss = 1024,
                    queryPort = 28215,
                    S05 = false,
                    Sk = 0,
                    EXTERNAL_SERVER_UID = "fac72328abc12cead5ddc71efa7f9d09",
                    Guid = "CD63318B4522C1C91CF2F6BDFFF5AD16",
                    Port = server.Port, //
                    se = 9
                }
            }
        };

        return Ok(r);
    }
}