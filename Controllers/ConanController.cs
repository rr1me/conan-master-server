using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.Models;
using conan_master_server.ServerLogic;
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
    private readonly SocketHandler _socketHandler;
    private readonly ServerHandler _serverHandler;

    public ConanController(PlayerData playerData, RequestHandler requestHandler, DatabaseContext db,
        RandomGenerator randomGenerator, ResponseWrapper wrapper, TokenGenerator tokenGenerator, SocketHandler socketHandler, ServerHandler serverHandler)
    {
        _playerData = playerData;
        _requestHandler = requestHandler;
        _db = db;
        _randomGenerator = randomGenerator;
        _wrapper = wrapper;
        _tokenGenerator = tokenGenerator;
        _socketHandler = socketHandler;
        _serverHandler = serverHandler;
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
    public IActionResult GetIdentityToken([FromBody] TokenRequest tokenRequest)
    {
        var ticket = tokenRequest.SessionTicket;

        var user = _db.Users.FirstOrDefault(x => x.Ticket == ticket);

        if (user == null)
            return BadRequest("No such user in db");

        var r = _tokenGenerator.Generate(_randomGenerator);

        user.Token = r.Token;
        _db.SaveChanges();
        
        _wrapper.data = new
        {
            FunctionResult = _tokenGenerator.Generate(_randomGenerator)
        };
        return Ok(_wrapper);
    }

    [HttpPost("auth")]
    public IActionResult Auth(TokenWrapped tokenWrapped)
    {
        var user = _db.Users.FirstOrDefault(x => x.Token == tokenWrapped.Token);

        if (user == null)
            return BadRequest("No such user in db");
        
        
        return Ok(new
        {
            data = new
            {
                FunctionResult = _playerData.CreateTitleInfo(user, user.SpecId)
            }
        });
    }

    [HttpPost("cloud")]
    public IActionResult CloudScript()
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

    [HttpGet("servers")]
    public IActionResult GetServers()
    {
        var servers = _db.Servers.ToList();
        return Ok(new
        {
            sessions = servers
        });
    }

    [HttpGet("ping")]
    public IActionResult Ping(string ip, int port)
    {
        var id = ip + ":" + port;
        var server = _db.Servers.FirstOrDefault(x => x.Id == id);
        
        if (server == null)
            return BadRequest(new
            {
                code = 400,
                status = "No such server in db to ping"
            });
        
        server.LastPing = DateTime.Now;
        _db.SaveChanges();
        return Ok(new
        {
            code = 200,
            status = "OK"
        });
    }

    [HttpGet("ws")]
    public async Task GetWs()
    {
        _serverHandler._db = _db;
        await _socketHandler.Handle(HttpContext, _serverHandler.InitialHandler);
    }
}