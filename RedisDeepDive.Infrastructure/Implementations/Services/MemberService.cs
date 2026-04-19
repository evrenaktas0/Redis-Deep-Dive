using MongoDB.Driver;
using RedisDeepDive.Application.Contracts.Requests;
using RedisDeepDive.Application.Interfaces.Services;
using RedisDeepDive.Domain.Entities;
using RedisDeepDive.Infrastructure.Persistence.Contexts;

namespace RedisDeepDive.Infrastructure.Implementations.Services;

public class MemberService:IMemberService
{
    private readonly MongoConnectionContext _context;

    public MemberService(MongoConnectionContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateAsync(MemberCreateRequest  memberCreateRequest)
    {
        try
        {
            MemberProfile memberProfile = new MemberProfile();
            memberProfile.MemberId = memberCreateRequest.MemberId;
            memberProfile.Name = memberCreateRequest.Name;
            memberProfile.IsAllowFollowing = memberCreateRequest.IsAllowFollowing;
            memberProfile.FollowerCount = memberCreateRequest.FollowerCount;
            memberProfile.FollowingCount = memberCreateRequest.FollowingCount;
            memberProfile.Username = memberCreateRequest.Username;
            await _context.MemberProfiles.InsertOneAsync(memberProfile);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public Task<MemberProfile> GetAsync(int memberId)
    {
        var filter = Builders<MemberProfile>.Filter.Eq(m => m.MemberId, memberId);
        return _context.MemberProfiles.Find(filter).FirstOrDefaultAsync();
    }

    public async Task IncrementFollowerCountAsync(int followerId, int following)
    {
        var followerTask = _context.MemberProfiles.UpdateOneAsync(
            m => m.MemberId == followerId, 
            Builders<MemberProfile>.Update.Inc(m => m.FollowingCount, 1));

        var followedTask = _context.MemberProfiles.UpdateOneAsync(
            m => m.MemberId == following, 
            Builders<MemberProfile>.Update.Inc(m => m.FollowerCount, 1));

        await Task.WhenAll(followerTask, followedTask);
    }
}