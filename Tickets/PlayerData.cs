using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.Entities;
using conan_master_server.Models;

namespace conan_master_server.Tickets;

public class PlayerData
{
    private readonly RequestHandler _requestHandler;

    public PlayerData(RequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }

    private readonly object stubObj = new { };
    private readonly IList<object> stubList = new List<object>();

    public async Task<object> Generate(long steamId, string titleId, DatabaseContext db, HttpContext httpContext, RandomGenerator randomGenerator)
    {
        var sessionTicket = MakeSessionTicket(randomGenerator, titleId, out var playFabId,
            out var publisherId, out var entityId,
            out var unknownId, out var endToken);
        
        var accountInfo = await _requestHandler.GetUserInfo(steamId);

        var user = db.Users.FirstOrDefault(x => x.SteamId == steamId);
        if (user == null)
        {
            // var date = DateTime.UtcNow;
            user = new ConanUser
            {
                SteamId = steamId,
                EntityId = entityId,
                Ip = httpContext.Request.Host.Host,
                PlayfabId = playFabId,
                PublisherId = publisherId,
                SpecId = titleId, //?
                Ticket = sessionTicket,
                Token = "token", //?
                Username = accountInfo.PersonaName, //?
                CreationDate = DateTime.UtcNow
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        return new
        {
            SessionTicket = sessionTicket,
            PlayFabId = playFabId,
            NewlyCreated = false,
            SettingsForUser = new
            {
                NeedsAttribution = false,
                GatherDeviceInfo = true,
                GatherFocusInfo = true
            },
            LastLoginTime = DateTime.UtcNow,
            InfoResultPayload = new
            {
                AccountInfo = await GetAccountInfo(user, titleId, accountInfo)
            },
            EntityToken = new
            {
                EntityToken = "token",
                TokenExpiration = "when",
                Entity = CreateTitle(entityId)
            },
            TreatmentAssignment = new
            {
                Variants = stubList,
                Variables = stubList
            }
        };
    }

    private string MakeSessionTicket(RandomGenerator randomGenerator, 
        string titleId, 
        out string playFabId, 
        out string publisherId,
        out string entityId, out string unknownId, out string endToken)
    {
        playFabId = randomGenerator.Generate16();
        publisherId = randomGenerator.Generate16();
        entityId = randomGenerator.Generate16();
        // titleId = randomGenerator.Generate5();

        unknownId = randomGenerator.Generate16();
        endToken = randomGenerator.GenerateEnd();

        return $"{playFabId}-{publisherId}-{entityId}-{titleId}-{unknownId}-{endToken}";
    }

    private async Task<object> GetAccountInfo(ConanUser user, string titleId, SteamPlayerInfo accountInfo)
    {
        // var accountInfo = await _requestHandler.GetUserInfo(user.SteamId);

        return new
        {
            PlayFabId = user.PlayfabId,
            Created = user.CreationDate,
            TitleInfo = CreateTitleInfo(user),
            PrivateInfo = stubObj,
            SteamInfo = new
            {
                SteamId = user.SteamId,
                SteamName = accountInfo.PersonaName,
                SteamCountry = accountInfo.LocCountryCode,
                SteamCurrency = "RUB"
            },
            UserInventory = stubList,
            UserData = stubObj,
            UserDataVersion = 149,
            UserReadOnlyDataVersion = 0,
            CharacterInventories = stubList,
            PlayerProfile = new
            {
                user.PublisherId,
                TitleId = titleId,
                PlayerId = user.PlayfabId,
                DisplayName = user.Username
            }
        };
    }

    private TitleInfo CreateTitleInfo(ConanUser user) =>
        new()
        {
            DisplayName = user.Username + user.Id,
            Created = user.CreationDate,
            LastLogin = DateTime.UtcNow,
            FirstLogin = user.CreationDate,
            IsBanned = false,
            TitlePlayerAccount = CreateTitle(user.EntityId)
        };

    private TitlePlayerAccount CreateTitle(string entityId) =>
        new()
        {
            Id = entityId,
        };
}