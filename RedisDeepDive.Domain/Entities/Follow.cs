using MongoDB.Bson;
using MongoDB.Driver;
namespace RedisDeepDive.Domain.Entities;

public class Follow
{
    public ObjectId Id { get; set; }
    public bool IsActive { get; set; }
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime CreatedAt { get; set; } 
}