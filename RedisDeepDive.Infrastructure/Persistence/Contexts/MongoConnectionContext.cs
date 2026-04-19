using RedisDeepDive.Domain.Entities;
using MongoDB.Driver;

namespace RedisDeepDive.Infrastructure.Persistence.Contexts;

public class MongoConnectionContext
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoConnectionContext()
    {
        var mongoConnection = new MongoClient();
        _mongoDatabase = mongoConnection.GetDatabase("RedisDeepDiveDB");
    }

    public IMongoCollection<MemberProfile> MemberProfiles => _mongoDatabase
        .GetCollection<MemberProfile>("MemberProfiles");

    public IMongoCollection<Follow> Follows => _mongoDatabase
        .GetCollection<Follow>("Follows");
}