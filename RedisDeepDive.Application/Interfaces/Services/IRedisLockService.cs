namespace RedisDeepDive.Application.Interfaces.Services;

public interface IRedisLockService
{
    Task<bool> TryAcquireLock(string key, string token, TimeSpan expiry);
    Task ReleaseLock(string key, string token);
}