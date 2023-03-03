namespace conan_master_server.Additional;

public class RandomGenerator
{
    private readonly Random _rand;
    public RandomGenerator()
    {
        _rand = new Random(Environment.TickCount);
    }

    private readonly string[] _randomChars = {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",
        "abcdefghijkmnopqrstuvwxyz",
        "0123456789",
        "!@$?_-"
    };

    public string Generate5() => Generate(5).ToUpper();
    public string Generate16() => Generate(16).ToUpper();
    public string GenerateEnd() => Generate(43) + "=";
    public string GenerateToken() => Generate(314);
    public string GenerateCounter() => Generate(32);

    private string Generate(int symbolCount)
    {
        var chars = new List<char>();
        for (var i = 0; i < symbolCount; i++)
        {
            var row = _rand.Next(0, 3); //3 = no !@$?_- symbols
            chars.Add(_randomChars[row][_rand.Next(0, _randomChars[row].Length)]);
        }

        return new string(chars.ToArray());
    }
}