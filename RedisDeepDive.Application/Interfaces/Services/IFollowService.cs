using RedisDeepDive.Application.Wrappers;

namespace RedisDeepDive.Application.Interfaces.Services;

public interface IFollowService
{
    Task<ServiceResponse> CreateFollow(int followerId, int followingId);
    Task<bool> IsAlreadyFollowingAsync(int followerId, int followingId);
}