namespace conan_master_server.Models;

public class BpRequest
{
    public long NativePlayerId { get; set; }
}

public class BpResponse
{
    public IEnumerable<string> DurablePlayerRewards { get; }

    public BpResponse(IEnumerable<string> durablePlayerRewards)
    {
        DurablePlayerRewards = durablePlayerRewards;
    }
}