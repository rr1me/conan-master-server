namespace conan_master_server.Models;

public class TokenRequest
{
    // public string FunctionName { get; set; }
    // public FunctionParameter FunctionParameter { get; set; }
    
    public string SessionTicket { get; set; }
    public string StoreName { get; set; }
    public string SkipWebRtcConfig { get; set; }
    public string AuthToken { get; set; }
}

// public class FunctionParameter
// {
//     public string SessionTicket { get; set; }
//     public string StoreName { get; set; }
//     public string SkipWebRtcConfig { get; set; }
//     public string AuthToken { get; set; }
// }