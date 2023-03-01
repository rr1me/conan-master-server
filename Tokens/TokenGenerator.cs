using conan_master_server.Additional;

namespace conan_master_server.Tokens;

public class TokenGenerator
{
    public TokenWrapped Generate(RandomGenerator generator)
    {
        return new TokenWrapped
        {
            Token = generator.GenerateToken(),
            Counter = generator.GenerateCounter()
        };
    }
}

public class TokenWrapped
{
    public string Token { get; set; }
    public string Counter { get; set; }
}