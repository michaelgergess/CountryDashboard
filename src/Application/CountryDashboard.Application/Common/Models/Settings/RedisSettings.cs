using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;

namespace CountryDashboard.Application.Common.Models.Settings
{
    public class RedisSettings
    {
        [Required]
        public string Host { get; set; } = null!;

        [Required]
        public int Port { get; set; }

        public string? User { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        public bool UseSsl { get; set; } = false;


        public ConfigurationOptions GetConfiguration()
        {
            var config = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                AllowAdmin = false,
                Ssl = UseSsl,
                User = User,
                Password = Password,
            };

            // Add endpoint
            config.EndPoints.Add(Host, Port);

            return config;
        }
    }
}
