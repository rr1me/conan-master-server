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
                Ip = httpContext.Request.Host.Host,
                PlayfabId = ticketObject.PlayFabId,
                PublisherId = ticketObject.PublisherId,
                SpecId = titleId, //?
                Ticket = ticketObject.SessionTicket,
                Token = randomGenerator.GenerateToken(), //?
                Username = accountInfo.PersonaName, //?
                CreationDate = DateTime.UtcNow
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        _loginData.SessionTicket = user.Ticket;
        _loginData.PlayFabId = user.PlayfabId;
        var irp = _loginData.InfoResultPayload;
        irp.AccountInfo = GetAccountInfo(user, accountInfo);
        irp.PlayerProfile = new
        {
            user.PublisherId,
            TitleId = titleId,
            PlayerId = user.PlayfabId,
            DisplayName = user.Username + randomGenerator.Generate5()
        };
        _loginData.EntityToken = new
        {
            EntityToken = "token",
            TokenExpiration = "when",
            Entity = new TitlePlayerAccount(user.EntityId)
        };
        return _loginData;
    }

    private object GetAccountInfo(ConanUser user, SteamPlayerInfo accountInfo) =>
        new
        {
            PlayFabId = user.PlayfabId,
            Created = user.CreationDate,
            TitleInfo = new TitleInfo(user),
            PrivateInfo = new { },
            SteamInfo = new
            {
                user.SteamId,
                SteamName = accountInfo.PersonaName,
                SteamCountry = accountInfo.LocCountryCode,
                SteamCurrency = "RUB"
            }
        };
}