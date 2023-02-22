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
    // [JsonProperty("steamid")]
    public long SteamId { get; set; }
    // [JsonProperty("personalname")]
    public string PersonaName { get; set; }
    // [JsonProperty("loccountrycode")]
    public string LocCountryCode { get; set; }
}