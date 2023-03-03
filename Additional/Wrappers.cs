using conan_master_server.Entities;

namespace conan_master_server.Additional;

class funcWrap
{
    public object FunctionResult { get; }

    public funcWrap(object functionResult)
    {
        FunctionResult = functionResult;
    }
}

class cloudResp
{
    public bool IsPlayerBanned => false;
    public object ChargebackAdjustedVirtualCurrencies { get; } = new { };
    public object BannedNegativeVirtualCurrencies { get; } = new { };
}

class sessions
{
    public List<ServerEntity> Sessions { get; }

    public sessions(List<ServerEntity> sessions)
    {
        Sessions = sessions;
    }
}