using Application.Common.Abstractions;
using StackExchange.Redis;

namespace Infrastructure.Caching;

public sealed class RedisOtpCache : IOtpCache
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisOtpCache(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task SetOtpAsync(string identifier, string otp, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        var key = GetKey(identifier);
        await _database.StringSetAsync(key, otp, expiration);
    }

    public async Task<string?> GetOtpAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var key = GetKey(identifier);
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task RemoveOtpAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var key = GetKey(identifier);
        await _database.KeyDeleteAsync(key);
    }

    private static string GetKey(string identifier) => $"otp:{identifier}";
}
