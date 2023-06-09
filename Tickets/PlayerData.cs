using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.Entities;
using conan_master_server.Models;

namespace conan_master_server.Tickets;

public class PlayerData
{
    private readonly RequestHandler _requestHandler;
    private readonly TicketHandler _ticketHandler;

    private readonly LoginData _loginData;

    public PlayerData(RequestHandler requestHandler, TicketHandler ticketHandler, LoginData loginData)
    {
        _requestHandler = requestHandler;
        _ticketHandler = ticketHandler;
        _loginData = loginData;
    }

    public async Task<LoginData> Generate(AuthTicket authTicket, DatabaseContext db, HttpContext httpContext,
        RandomGenerator randomGenerator)
    {
        var titleId = authTicket.TitleId;
        var steamResponse = await _requestHandler.AuthUserTicket(authTicket.SteamTicket);
        var steamId = steamResponse.SteamId;
        var accountInfo = await _requestHandler.GetUserInfo(steamId);

        var user = db.Users.FirstOrDefault(x => x.SteamId == steamId);
        if (user == null)
        {
            var ticketObject = _ticketHandler.MakeSessionTicket(titleId, randomGenerator);
            user = new ConanUser
            {
                SteamId = steamId,
                EntityId = ticketObject.EntityId,
                Ip = httpContext.Connection.RemoteIpAddress.ToString(),
                PlayfabId = ticketObject.PlayFabId,
                PublisherId = ticketObject.PublisherId,
                SpecId = titleId, //?
                Ticket = ticketObject.SessionTicket,
                Token = randomGenerator.GenerateToken(), //?
                Username = accountInfo.PersonaName, //?
                CreationDate = DateTime.UtcNow,
                Identifier = randomGenerator.Generate5(),
                BpLevel = 1
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        _loginData.SessionTicket = user.Ticket;
        _loginData.PlayFabId = user.PlayfabId;
        var irp = _loginData.InfoResultPayload;
        irp.AccountInfo = new AccountInfo(user, accountInfo);
        irp.PlayerProfile = new PlayerProfile(user);
        _loginData.EntityToken = new EntityTokenWrapper(new TitlePlayerAccount(user.EntityId));
        return _loginData;
    }

    public async Task<string> LicenseCheck(TokenWrapped tokenWrapped) =>
        await _requestHandler.MakeFancomRequest(tokenWrapped);

    public bool TryGetBpItems(DatabaseContext db, long SteamId, out IEnumerable<string>? items)
    {
        var user = db.Users.FirstOrDefault(x => x.SteamId == SteamId);

        if (user == null)
        {
            items = null;
            return false;
        }
        
        var battlePass = db.BattlePasses.FirstOrDefault(x => x.Level == user.BpLevel);

        items = battlePass.Items.Split(',').Select(x => $"{{Id3-{x}}}");
        return true;
    }
}