namespace conan_master_server.Models;

public class TitleInfo
{
    public TitleInfo()
    {
        Origination = "Steam";
    }

    public string DisplayName { get; set; }
    public string Origination { get; }
    public DateTime Created { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime FirstLogin { get; set; }
    public bool IsBanned { get; set; }
    public TitlePlayerAccount TitlePlayerAccount { get; set; }
}

public class TitlePlayerAccount
{
    public TitlePlayerAccount()
    {
        Type = "title_player_account";
        TypeString = "title_player_account";
    }
    
    public string Id { get; set; }
    public string Type { get; }
    public string TypeString { get; }
}