using conan_master_server.Entities;

namespace conan_master_server.Models;

public class TitleInfo
{
    public string DisplayName { get; }
    public string Origination => "Steam";
    public DateTime Created { get; }
    public DateTime LastLogin { get; }
    public DateTime FirstLogin { get; }
    public bool IsBanned => false;
    public TitlePlayerAccount TitlePlayerAccount { get; }

    public TitleInfo(ConanUser user)
    {
        DisplayName = user.Username + user.Identifier;
        Created = user.CreationDate;
        LastLogin = DateTime.UtcNow;
        FirstLogin = user.CreationDate;
        TitlePlayerAccount = new TitlePlayerAccount(user.EntityId);
    }
}

public class TitlePlayerAccount
{
    public string Id { get; }
    public string Type => "title_player_account";
    public string TypeString => "title_player_account";

    public TitlePlayerAccount(string id)
    {
        Id = id;
    }
}