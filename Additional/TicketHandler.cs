﻿namespace conan_master_server.Additional;

public class TicketHandler
{
    public ticketObject MakeSessionTicket(string titleId, RandomGenerator randomGenerator)
    {
        var playFabId = randomGenerator.Generate16();
        var publisherId = randomGenerator.Generate16();
        var entityId = randomGenerator.Generate16();
        
        var unknownId = randomGenerator.Generate16();
        var endToken = randomGenerator.GenerateEnd();
        
        return new ticketObject
        {
            PlayFabId = playFabId,
            PublisherId = publisherId,
            EntityId = entityId,
            SessionTicket = $"{playFabId}-{publisherId}-{entityId}-{titleId}-{unknownId}-{endToken}"
        };;
    }
}

public class ticketObject
{
    public string PlayFabId { get; init; }
    public string PublisherId { get; init; }
    public string EntityId { get; init; }
    public string SessionTicket { get; init; }
}