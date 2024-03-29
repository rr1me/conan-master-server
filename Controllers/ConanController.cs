﻿using conan_master_server.Additional;
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

    private readonly ILogger<ConanController> _logger;

    public ConanController(PlayerData playerData, DatabaseContext db, RandomGenerator randomGenerator,
        ResponseWrapper wrapper, SocketHandler socketHandler, ServerHandler serverHandler, ILogger<ConanController> logger)
    {
        _playerData = playerData;
        _db = db;
        _randomGenerator = randomGenerator;
        _wrapper = wrapper;
        _socketHandler = socketHandler;
        _serverHandler = serverHandler;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthTicket authTicket)
    {
        try
        {
            _wrapper.data = await _playerData.Generate(authTicket, _db, HttpContext, _randomGenerator);
        }
        catch (ArgumentException e)
        {
            return StatusCode(406, e.Message);
        }
        return Ok(_wrapper);
    }

    [HttpPost("token")]
    public IActionResult GetIdentityToken([FromBody] TokenRequest tokenRequest)
    {
        var ticket = tokenRequest.SessionTicket;

        var user = _db.Users.FirstOrDefault(x => x.Ticket == ticket);

        if (user == null)
            return StatusCode(410, "No such user in db");

        user.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
        _db.SaveChanges();

        _wrapper.data = new funcWrap(new TokenWrapped
        {
            Token = user.Token,
            Counter = _randomGenerator.GenerateCounter()
        });
        return Ok(_wrapper);
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Auth([FromBody] TokenWrapped tokenWrapped)
    {
        var user = _db.Users.FirstOrDefault(x => x.Token == tokenWrapped.Token);

        if (user == null)
        {
            var response = await _playerData.LicenseCheck(tokenWrapped);
            return response.Contains("MAIN_TITLE_STAGING") ? Ok(response) : StatusCode(410, "No such user in db");
        }

        _wrapper.data = new funcWrap(new AuthResponse(user));
        return Ok(_wrapper);
    }

    [HttpPost("cloud")]
    public IActionResult CloudScript()
    {
        _wrapper.data = new funcWrap(new cloudResp());
        return Ok(_wrapper);
    }

    [HttpPost("battlepass")]
    public IActionResult BattlePass([FromBody]BpRequest bpRequest)
    {
        if (!_playerData.TryGetBpItems(_db, bpRequest.NativePlayerId, out var items))
            return StatusCode(410, "No such user in db");
            
        _wrapper.data = new funcWrap(new BpResponse(items));
        return Ok(_wrapper);
    }

    [HttpGet("servers")]
    public IActionResult GetServers()
    {
        var servers = _db.Servers.ToList();
        return Ok(new sessions(servers));
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping(int port)
    {
        var remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
        
        var id = remoteIp + ":" + port;
        _logger.LogInformation($"Pinging: {id}");
        var server = _db.Servers.FirstOrDefault(x => x.Id == id);

        if (server == null)
            return StatusCode(410, "No such server in db.");

        server.LastPing = DateTime.Now;
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("ws")]
    public async Task GetWs()
    {
        await _socketHandler.Handle(HttpContext, (x, y) => _serverHandler.InitialHandler(x, y, _db));
    }
}