using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.ModelBinder;
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

    public ConanController(PlayerData playerData, RequestHandler requestHandler, DatabaseContext db, RandomGenerator randomGenerator, ResponseWrapper wrapper, TokenGenerator tokenGenerator)
    {
        _playerData = playerData;
        _requestHandler = requestHandler;
        _db = db;
        _randomGenerator = randomGenerator;
        _wrapper = wrapper;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthTicket authTicket)
    {
        var steamResponse = await _requestHandler.AuthUserTicket(authTicket.SteamTicket);

        _wrapper.data = await _playerData.Generate(steamResponse.SteamId, authTicket.TitleId, _db, HttpContext,
            _randomGenerator);
        return Ok(_wrapper);

        return Ok(new
        {
            code = 200,
            status = "OK",
            data = await _playerData.Generate(steamResponse.SteamId, authTicket.TitleId, _db, HttpContext, _randomGenerator)
        });
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetIdentityToken([FromBody]TokenRequest tokenRequest)
    {
        // Console.WriteLine(body);
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
}