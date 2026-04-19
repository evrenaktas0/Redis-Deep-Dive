using MongoDB.Bson;

namespace RedisDeepDive.Domain.Entities;

public class MemberProfile
{
    public ObjectId Id { get; set; }
    public int MemberId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsAllowFollowing { get; set; }
    // Beni takip edenlerin sayısı
    public int FollowerCount { get; set; }
    // Benim takip ettiğim kişilerin sayısı
    public int FollowingCount { get; set; }
}