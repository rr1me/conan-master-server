namespace conan_master_server.Additional;

public class ResponseWrapper
{
    public ResponseWrapper()
    {
        code = 200;
        status = "OK";
    }

    public int code { get; }
    public string status { get; }
    public object data { get; set; }
}