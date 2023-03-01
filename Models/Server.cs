using System.ComponentModel.DataAnnotations;

namespace conan_master_server.Models;

// public class ServerTrash
// {
//     public decimal HungerRatio { get; set; }
//     public decimal ItemConvertRatio { get; set; }
//     public bool GameMode { get; set; }
//     public bool LostItemsAfterDying { get; set; }
//     public decimal DurabilityRatio { get; set; }
//     public bool IsPrivate { get; set; }
//     public decimal SlavingRatio { get; set; }
//     public int UniquePlayers { get; set; }
//     public bool IsVac { get; set; }
//     public decimal YieldRatio { get; set; }
//     public int BuildId { get; set; }
//     public string Mods { get; set; }
//     public int MaxPlayers { get; set; }
//     public decimal ExpRatio { get; set; }
//     public string ServerUID { get; set; }
//     public string Name { get; set; }
//     public bool CSF { get; set; }
//     public int IncarnationTime { get; set; }
//     public decimal ThirstRatio { get; set; }
//     public decimal HungerRatioNonActive { get; set; }
//     public decimal ThirstRatioNonActive { get; set; }
//     public int AfkTime { get; set; }
//     public decimal ResourceRecoveryRatio { get; set; }
//     public bool RestrictBuildDamage { get; set; }
//     public bool AllowToGetDiedPlayerItems { get; set; }
//     public int TimeBetweenSunriseAndSunset { get; set; }
//     public string MapName { get; set; }
//     public int DisplayedMaxPlayers { get; set; }
//     public decimal PowerWasteRatio { get; set; }
//     public int MaxAllowedStringInChat { get; set; }
//     public int QueryPort { get; set; }
//     public bool BattleEye { get; set; }
//     public decimal WastingFoodSpeed { get; set; }
//     public int Port { get; set; }
// }
//
// public class Server
// {
//     [Key]
//     public string Ip { get; set; }
//     public int MaxPlayers { get; set; }
//     public string Name { get; set; }
//     public string MapName { get; set; }
//     public int Port { get; set; }
//     public bool Pvp { get; set; }
// }

public class EbaniyServer
{
    [Key]
    public string Id { get; set; }
    public string s9 { get; set; }
    public decimal s8 { get; set; }
    public decimal s24 { get; set; }
    public bool s119 { get; set; }
    public bool s117 { get; set; }
    public string ip { get; set; }
    public bool s0 { get; set; }
    public bool s7 { get; set; }
    public decimal s6 { get; set; }
    public bool Private { get; set; }
    public decimal s4 { get; set; }
    public decimal s15 { get; set; }
    public bool s18 { get; set; }
    public decimal sl { get; set; }
    public int buildId { get; set; }
    public string s17 { get; set; }
    public decimal s30 { get; set; }
    public int maxplayers { get; set; }
    public string kdsUri { get; set; }
    public decimal sz { get; set; }
    public decimal sy { get; set; }
    public string serverUID { get; set; }
    public string name { get; set; }
    public bool s122 { get; set; }
    public string s120 { get; set; }
    public bool csf { get; set; }
    public string sw { get; set; }
    public string su { get; set; }
    public decimal s22 { get; set; }
    public decimal s23 { get; set; }
    public decimal s21 { get; set; }
    public int so { get; set; }
    public decimal sm { get; set; }
    public bool s25 { get; set; }
    public bool sa { get; set; }
    public decimal sg { get; set; }
    public decimal sf { get; set; }
    public string mapName { get; set; }
    public string displayedmaxplayers { get; set; }
    public decimal sj { get; set; }
    public decimal ss { get; set; }
    public int queryPort { get; set; }
    public bool s05 { get; set; }
    public decimal sk { get; set; }
    public string externaL_SERVER_UID { get; set; }
    public string guid { get; set; }
    public int port { get; set; }
    public string se { get; set; }
}