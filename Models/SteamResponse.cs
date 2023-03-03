namespace conan_master_server.Models;

public class Params
{
    public string Result { get; set; }
    public long SteamId { get; set; }
    public long OwnerSteamId { get; set; }
    public bool VacBanned { get; set; }
    public bool PublisherBanned { get; set; }
}

public class SteamPlayerInfo
{
    public long SteamId { get; set; }
    public string PersonaName { get; set; }
    public string LocCountryCode { get; set; }
}