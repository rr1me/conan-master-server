using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.Models;
using conan_master_server.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace conan_master_server.Controllers;

public class ConanController : ControllerBase
{
    private readonly PlayerData _playerData;
    private readonly RequestHandler _requestHandler;
    private readonly DatabaseContext _db;
    private readonly RandomGenerator _randomGenerator;

    public ConanController(PlayerData playerData, RequestHandler requestHandler, DatabaseContext db, RandomGenerator randomGenerator)
    {
        _playerData = playerData;
        _requestHandler = requestHandler;
        _db = db;
        _randomGenerator = randomGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthTicket authTicket)
    {
        var steamResponse = await _requestHandler.AuthUserTicket(authTicket.SteamTicket);

        return Ok(new
        {
            code = 200,
            status = "OK",
            data = await _playerData.Generate(steamResponse.SteamId, authTicket.TitleId, _db, HttpContext, _randomGenerator)
        });
    }
}