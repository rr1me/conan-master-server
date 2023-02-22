namespace conan_master_server.Models;

public class AuthTicket
{
    public bool CreateAccount { get; set; }
    public InfoRequestParameters InfoRequestParameters { get; set; }
    public string SteamTicket { get; set; }
    public string TitleId { get; set; }
}