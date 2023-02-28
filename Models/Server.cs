using System.ComponentModel.DataAnnotations;

namespace conan_master_server.Models;

public class ServerTrash
{
    public float HungerRatio { get; set; }
    public float ItemConvertRatio { get; set; }
    public bool GameMode { get; set; }
    public bool LostItemsAfterDying { get; set; }
    public float DurabilityRatio { get; set; }
    public bool IsPrivate { get; set; }
    public float SlavingRatio { get; set; }
    public int UniquePlayers { get; set; }
    public bool IsVac { get; set; }
    public float YieldRatio { get; set; }
    public int BuildId { get; set; }
    public string Mods { get; set; }
    public int MaxPlayers { get; set; }
    public float ExpRatio { get; set; }
    public string ServerUID { get; set; }
    public string Name { get; set; }
    public bool CSF { get; set; }
    public int IncarnationTime { get; set; }
    public float ThirstRatio { get; set; }
    public float HungerRatioNonActive { get; set; }
    public float ThirstRatioNonActive { get; set; }
    public int AfkTime { get; set; }
    public float ResourceRecoveryRatio { get; set; }
    public bool RestrictBuildDamage { get; set; }
    public bool AllowToGetDiedPlayerItems { get; set; }
    public int TimeBetweenSunriseAndSunset { get; set; }
    public string MapName { get; set; }
    public int DisplayedMaxPlayers { get; set; }
    public float PowerWasteRatio { get; set; }
    public int MaxAllowedStringInChat { get; set; }
    public int QueryPort { get; set; }
    public bool BattleEye { get; set; }
    public float WastingFoodSpeed { get; set; }
    public int Port { get; set; }
}

public class Server
{
    [Key]
    public string Ip { get; set; }
    public int MaxPlayers { get; set; }
    public string Name { get; set; }
    public string MapName { get; set; }
    public int Port { get; set; }
    public bool Pvp { get; set; }
}