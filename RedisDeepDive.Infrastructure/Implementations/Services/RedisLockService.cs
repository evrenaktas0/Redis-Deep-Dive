using RedisDeepDive.Application.Interfaces.Services;
using RedisDeepDive.Infrastructure.Persistence.Contexts;
using StackExchange.Redis;

namespace RedisDeepDive.Infrastructure.Implementations.Services;

public class RedisLockService:IRedisLockService
{
    private readonly IDatabase _redisDatabase;
    private readonly RedisConnectionContext _connectionContext;
    public RedisLockService(RedisConnectionContext connectionContext)
    {
        _connectionContext = connectionContext;
        _redisDatabase = _connectionContext.GetDatabase();
    }
    public async Task<bool> TryAcquireLock(string key, string token, TimeSpan expiry)
    {
        return await _redisDatabase.StringSetAsync(key, token, expiry, When.NotExists);
    }
    public async Task ReleaseLock(string key, string token)
    {
        string luaScript = @"
            if redis.call('get', KEYS[1]) == ARGV[1] then
                return redis.call('del', KEYS[1])
            else
                return 0
            end";
        await _redisDatabase.ScriptEvaluateAsync(luaScript, new RedisKey[] { key }, new RedisValue[] { token });
    }
}