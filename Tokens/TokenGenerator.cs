namespace conan_master_server.Tokens;

public class TokenGenerator
{
    public TokenResponse Generate()
    {
        return new TokenResponse
        {
            Token = "token",
            Counter = "Counter"
        };
    }
}

public class TokenResponse
{
    public string Token { get; set; }
    public string Counter { get; set; }
}