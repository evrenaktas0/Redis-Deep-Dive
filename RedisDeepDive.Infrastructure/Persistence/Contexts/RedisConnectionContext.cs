using StackExchange.Redis;

namespace RedisDeepDive.Infrastructure.Persistence.Contexts;

public class RedisConnectionContext
{
    private readonly IDatabase _redisDatabase;
    public RedisConnectionContext(IConnectionMultiplexer connectionMultiplexer)
    {
        _redisDatabase = connectionMultiplexer.GetDatabase();
    }
    public IDatabase GetDatabase()
    {
         return _redisDatabase;
    }
}