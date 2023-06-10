namespace conan_master_server.Models;

public class BpRequest
{
    public long NativePlayerId { get; set; }
}

public class BpResponse
{
    public IEnumerable<BpItem> DurablePlayerRewards { get; }

    public BpResponse(IEnumerable<BpItem> durablePlayerRewards)
    {
        DurablePlayerRewards = durablePlayerRewards;
    }
}

public class BpItem
{
    public string Id { get; }

    public BpItem(string id)
    {
        Id = id;
    }
}