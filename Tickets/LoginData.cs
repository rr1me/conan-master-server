namespace conan_master_server.Tickets;

public class LoginData
{
    private readonly IList<object> stubList = new List<object>();

    public string SessionTicket { get; set; }
    public string PlayFabId { get; set; }

    public bool NewlyCreated { get; } = false;
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
    public bool NeedsAttribution { get; } = false;
    public bool GatherDeviceInfo { get; } = true;
    public bool GatherFocusInfo { get; } = true;
}

public class InfoResultPayload
{
    public object AccountInfo { get; set; }
    public IList<object> UserInventory { get; set; }
    public object UserData { get; set; }
    public int UserDataVersion { get; } = 149;
    public int UserReadOnlyDataVersion { get; } = 0;
    public IList<object> CharacterInventories { get; set; }
    public object PlayerProfile { get; set; }
}

public class TreatmentAssignment
{
    public IList<object> Variants { get; set; }
    public IList<object> Variables { get; set; }
}