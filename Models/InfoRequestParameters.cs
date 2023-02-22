namespace conan_master_server.Models;

public class InfoRequestParameters
{
    public bool GetCharacterInventories { get; set; }
    public bool GetCharacterList { get; set; }
    public bool GetPlayerProfile { get; set; }
    public bool GetPlayerStatistics { get; set; }
    public bool GetTitleData { get; set; }
    public bool GetUserAccountInfo { get; set; }
    public bool GetUserData { get; set; }
    public bool GetUserInventory { get; set; }
    public bool GetUserReadOnlyData { get; set; }
    public bool GetUserVirtualCurrency { get; set; }
}