namespace conan_master_server.Additional;

public class ResponseWrapper
{
    public int code => 200;
    public string status => "OK";
    public object data { get; set; }
}