namespace conan_master_server.Models;

public class TokenRequest
{
    public string SessionTicket { get; set; }
    public string StoreName { get; set; }
    public string SkipWebRtcConfig { get; set; }
    public string AuthToken { get; set; }
}