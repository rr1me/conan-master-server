using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.Models;
using conan_master_server.ServerLogic;
using conan_master_server.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace conan_master_server.Controllers;

public class ConanController : ControllerBase
{
    private readonly PlayerData _playerData;
    private readonly DatabaseContext _db;
    private readonly RandomGenerator _randomGenerator;
    private readonly ResponseWrapper _wrapper;
    private readonly SocketHandler _socketHandler;
    private readonly ServerHandler _serverHandler;
    private readonly CleanerOrchestrator _cleanerOrchestrator;

    public ConanController(PlayerData playerData, DatabaseContext db, RandomGenerator randomGenerator,
        ResponseWrapper wrapper, SocketHandler socketHandler, ServerHandler serverHandler, CleanerOrchestrator cleanerOrchestrator)
    {
        _playerData = playerData;
        _db = db;
        _randomGenerator = randomGenerator;
        _wrapper = wrapper;
        _socketHandler = socketHandler;
        _serverHandler = serverHandler;
        _cleanerOrchestrator = cleanerOrchestrator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthTicket authTicket)
    {
        _wrapper.data = await _playerData.Generate(authTicket, _db, HttpContext, _randomGenerator);
        return Ok(_wrapper);
    }

    [HttpPost("token")]
    public IActionResult GetIdentityToken([FromBody] TokenRequest tokenRequest)
    {
        var ticket = tokenRequest.SessionTicket;

        var user = _db.Users.FirstOrDefault(x => x.Ticket == ticket);

        if (user == null)
            return BadRequest("No such user in db");

        _wrapper.data = new funcWrap(new TokenWrapped
        {
            Token = user.Token,
            Counter = _randomGenerator.GenerateCounter()
        });
        return Ok(_wrapper);
    }

    [HttpPost("auth")]
    public IActionResult Auth([FromBody] TokenWrapped tokenWrapped)
    {
        var user = _db.Users.FirstOrDefault(x => x.Token == tokenWrapped.Token);

        if (user == null)
            return BadRequest("No such user in db");

        _wrapper.data = new funcWrap(new AuthResponse(user));
        return Ok(_wrapper);
    }

    [HttpPost("cloud")]
    public IActionResult CloudScript()
    {
        _wrapper.data = new funcWrap(new cloudResp());
        return Ok(_wrapper);
    }

    [HttpGet("servers")]
    public IActionResult GetServers()
    {
        var servers = _db.Servers.ToList();
        return Ok(new sessions(servers));
    }

    [HttpGet("ping")]
    public IActionResult Ping(string ip, int port)
    {
        var id = ip + ":" + port;
        var server = _db.Servers.FirstOrDefault(x => x.Id == id);

        if (server == null)
            return BadRequest("No such server in db to ping");

        server.LastPing = DateTime.Now;
        _db.SaveChanges();
        return Ok();
    }

    [HttpGet("ws")]
    public async Task GetWs()
    {
        await _socketHandler.Handle(HttpContext, x => _serverHandler.InitialHandler(x, _db));
        _cleanerOrchestrator.Run();
    }
}