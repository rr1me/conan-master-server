using conan_master_server.Entities;

namespace conan_master_server.Models;

public class TitleInfo
{
    public string DisplayName { get; set; }
    public string Origination { get; } = "Steam";
    public DateTime Created { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime FirstLogin { get; set; }
    public bool IsBanned { get; } = false;
    public TitlePlayerAccount TitlePlayerAccount { get; set; }

    public TitleInfo(ConanUser user)
    {
        DisplayName = user.Username + user.SpecId;
        Created = user.CreationDate;
        LastLogin = DateTime.UtcNow;
        FirstLogin = user.CreationDate;
        TitlePlayerAccount = new TitlePlayerAccount(user.EntityId);
    }
}

public class TitlePlayerAccount
{
    public string Id { get; }
    public string Type { get; } = "title_player_account";
    public string TypeString { get; } = "title_player_account";

    public TitlePlayerAccount(string id)
    {
        Id = id;
    }
}