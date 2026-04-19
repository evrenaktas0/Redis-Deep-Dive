using RedisDeepDive.Application.Contracts.Requests;
using RedisDeepDive.Domain.Entities;

namespace RedisDeepDive.Application.Interfaces.Services;

public interface IMemberService
{
    Task<bool> CreateAsync(MemberCreateRequest   memberCreateRequest);
    Task<MemberProfile> GetAsync(int memberId);
    Task IncrementFollowerCountAsync(int followerId, int following);
}