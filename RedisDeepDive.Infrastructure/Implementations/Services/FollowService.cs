
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RedisDeepDive.Application.Interfaces.Services;
using RedisDeepDive.Application.Wrappers;
using RedisDeepDive.Domain.Entities;
using RedisDeepDive.Infrastructure.Persistence.Contexts;

namespace RedisDeepDive.Infrastructure.Implementations.Services;

public class FollowService:IFollowService
{
    private readonly MongoConnectionContext _context;
    private readonly IMemberService _memberService;
    private readonly IConfiguration _configuration;
    private int maxFollowingLimit = 0;
    public FollowService(MongoConnectionContext context, IMemberService memberService, IConfiguration configuration)
    {
        _context = context;
        _memberService = memberService;
        _configuration = configuration;
        maxFollowingLimit=_configuration.GetValue<int>("MaxFollowingLimit");
    }
     
    public async Task<ServiceResponse> CreateFollow(int followerId, int followingId)
    {
        if (followerId == followingId)
            return ServiceResponse.Failure("Kendinizi takip edemezsiniz.");

        try
        {
            var member = await _memberService.GetAsync(followingId);
        
            if (member is null)
                return ServiceResponse.Failure("Takip edilmek istenen kullanıcı bulunamadı.");

            if (!member.IsAllowFollowing)
                return ServiceResponse.Failure("Bu kullanıcı yeni takipçi kabul etmiyor.");

            if (member.FollowerCount >= maxFollowingLimit)
                return ServiceResponse.Failure("Bu kullanıcının takipçi limiti dolmuştur.");

            if (await IsAlreadyFollowingAsync(followerId, followingId))
                return ServiceResponse.Failure("Bu kullanıcıyı zaten takip ediyorsunuz.");

            
            var follow = new Follow { FollowerId = followerId, FollowingId = followingId ,IsActive =  true ,CreatedAt =  DateTime.UtcNow};
            await _context.Follows.InsertOneAsync(follow);
            await _memberService.IncrementFollowerCountAsync(followerId, followingId);

            return ServiceResponse.Success();
        }
        catch (Exception ex)
        {
            return ServiceResponse.Failure("Sistemsel bir hata oluştu, lütfen sonra tekrar deneyiniz.");
        }
    }
     
    public async Task<bool> IsAlreadyFollowingAsync(int followerId, int followingId)
    {
        return await _context.Follows
            .Find(f => f.FollowerId == followerId && f.FollowingId == followingId && f.IsActive)
            .AnyAsync();
    }
}