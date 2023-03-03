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

    public int Generate5() => int.Parse(Generate(5, true));
    public string Generate16() => Generate(16).ToUpper();
    public string GenerateEnd() => Generate(43) + "=";
    public string GenerateToken() => Generate(314);
    public string GenerateCounter() => Generate(32);

    private string Generate(int symbolCount, bool onlyDigits = false)
    {
        var chars = new List<char>();
        for (var i = 0; i < symbolCount; i++)
        {
            var row = onlyDigits ? 2 : _rand.Next(0, 3); //3 = no !@$?_- symbols
            chars.Add(_randomChars[row][_rand.Next(0, _randomChars[row].Length)]);
        }

        return new string(chars.ToArray());
    }
}