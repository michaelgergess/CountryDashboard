using StackExchange.Redis;
using System.Text.Json;
using CountryDashboard.Application.Common.Interfaces.Services;

namespace CountryDashboard.Caching.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisCacheService(IConnectionMultiplexer multiplexer)
        {
            _db = multiplexer.GetDatabase();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            if (value == null)
                return;

            var json = JsonSerializer.Serialize(value, _jsonOptions);

            // Normal Redis string storage with TTL
            await _db.StringSetAsync(key, json, expiration).ConfigureAwait(false);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key).ConfigureAwait(false);

            if (value.IsNullOrEmpty)
                return default;

            var json = (string)value;
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key).ConfigureAwait(false);
        }
    }
}
