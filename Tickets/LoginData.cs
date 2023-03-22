using conan_master_server.Entities;
using conan_master_server.Models;

namespace conan_master_server.Tickets;

public class LoginData
{
    private readonly IList<object> stubList = new List<object>();

    public string SessionTicket { get; set; }
    public string PlayFabId { get; set; }

    public bool NewlyCreated => false;
    public SettingsForUser SettingsForUser { get; }
    public DateTime LastLoginTime { get; } = DateTime.Now;
    public InfoResultPayload InfoResultPayload { get; }

    public object EntityToken { get; set; }
    public TreatmentAssignment TreatmentAssignment { get; }

    public LoginData(InfoResultPayload infoResultPayload, SettingsForUser settingsForUser,
        TreatmentAssignment treatmentAssignment)
    {
        InfoResultPayload = infoResultPayload;
        InfoResultPayload.UserInventory = stubList;
        
        SettingsForUser = settingsForUser;

        TreatmentAssignment = treatmentAssignment;
        TreatmentAssignment.Variants = stubList;
        TreatmentAssignment.Variables = stubList;
    }
}

public class SettingsForUser
{
    public bool NeedsAttribution => false;
    public bool GatherDeviceInfo => true;
    public bool GatherFocusInfo => true;
}

public class InfoResultPayload
{
    public object AccountInfo { get; set; }
    public IList<object> UserInventory { get; set; }
    public object UserData { get; set; }
    public int UserDataVersion => 149;
    public int UserReadOnlyDataVersion => 0;
    public IList<object> CharacterInventories { get; set; }
    public object PlayerProfile { get; set; }
}

public class TreatmentAssignment
{
    public IList<object> Variants { get; set; }
    public IList<object> Variables { get; set; }
}

public class AccountInfo
{
    public string PlayFabId { get; }
    public DateTime Created { get; }
    public TitleInfo TitleInfo { get; }
    public object PrivateInfo { get; } = new { };
    public SteamInfo SteamInfo { get; }

    public AccountInfo(ConanUser user, SteamPlayerInfo info)
    {
        PlayFabId = user.PlayfabId;
        Created = user.CreationDate;
        TitleInfo = new TitleInfo(user);
        SteamInfo = new SteamInfo(user, info);
    }
}

public class SteamInfo
{
    public long SteamId { get; }
    public string SteamName { get; }
    public string SteamCountry { get; }
    public string SteamCurrency => "RUB";

    public SteamInfo(ConanUser user, SteamPlayerInfo info)
    {
        SteamId = user.SteamId;
        SteamName = info.PersonaName;
        SteamCountry = info.LocCountryCode;
    }
}

public class PlayerProfile
{
    public string PublisherId { get; }
    public string TitleId { get; }
    public string PlayerId { get; }
    public string DisplayName { get; }

    public PlayerProfile(ConanUser user)
    {
        PublisherId = user.PublisherId;
        TitleId = user.SpecId;
        PlayerId = user.PlayfabId;
        DisplayName = user.Username + user.Identifier;
    }
}

public class EntityTokenWrapper
{
    public string EntityToken => "Unknown field";
    public string TokenExpiration => "Unknown field";
    public TitlePlayerAccount Entity { get; }

    public EntityTokenWrapper(TitlePlayerAccount entity)
    {
        Entity = entity;
    }
}

public class TokenWrapped
{
    public string Token { get; set; }
    public string Counter { get; set; }
}

public class AuthResponse
{
    public string PlayFabId { get; }
    public TitleInfo TitleInfo { get; }
    public string STEAM { get; }
    public List<Entitlement> Entitlements { get; }
    public bool IsBanned => false;

    public AuthResponse(ConanUser user)
    {
        PlayFabId = user.PlayfabId;
        TitleInfo = new TitleInfo(user);
        STEAM = user.SteamId.ToString();
        Entitlements = new List<Entitlement>
        {
            new()
            {
                Id = "DLC_Siptah",
                Name = "DLC_Siptah"
            },
            new()
            {
                Id = "MAIN_TITLE_STAGING",
                Name = "MAIN_TITLE_STAGING"
            },
            new()
            {
                Id = "MAIN_TITLE",
                Name = "MAIN_TITLE"
            }
        };
    }
}

public class Entitlement
{
    public string Id { get; init; }
    public string Name { get; init; }
}