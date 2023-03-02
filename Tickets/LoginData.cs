namespace conan_master_server.Tickets;

public class LoginData
{
    private readonly object stubObj = new { };
    private readonly IList<object> stubList = new List<object>();
    
    public string SessionTicket { get; set; }
    public string PlayFabId { get; set; }
    
    public bool NewlyCreated { get; }
    public SettingsForUser SettingsForUser { get; }
    public DateTime LastLoginTime { get; }
    public InfoResultPayload InfoResultPayload { get; }
    
    public object EntityToken { get; set; }
    public TreatmentAssignment TreatmentAssignment { get; }

    public LoginData(InfoResultPayload infoResultPayload, SettingsForUser settingsForUser, TreatmentAssignment treatmentAssignment)
    {
        InfoResultPayload = infoResultPayload;
        InfoResultPayload.UserInventory = stubList;
        
        NewlyCreated = false;
        SettingsForUser = settingsForUser;
        LastLoginTime = DateTime.Now;
        
        TreatmentAssignment = treatmentAssignment;
        TreatmentAssignment.Variants = stubList;
        TreatmentAssignment.Variables = stubList;
    }
}

public class SettingsForUser
{
    public bool NeedsAttribution = false;
    public bool GatherDeviceInfo = true;
    public bool GatherFocusInfo = true;
}

public class InfoResultPayload
{
    public object AccountInfo { get; set; }
    public IList<object> UserInventory { get; set; }
    public object UserData {get;set;}
    public int UserDataVersion {get;}
    public int UserReadOnlyDataVersion {get;}
    public IList<object> CharacterInventories {get;set;}
    public object PlayerProfile {get;set;}

    public InfoResultPayload()
    {
        UserDataVersion = 149;
        UserReadOnlyDataVersion = 0;
    }
}

public class TreatmentAssignment
{
    public IList<object> Variants { get; set; }
    public IList<object> Variables { get; set; }
}