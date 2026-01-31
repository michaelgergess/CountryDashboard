using CountryDashboard.Authentication.Middleware;
using CountryDashboard.Integrations;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace CountryDashboard.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.Configure<FileAttachmentSettings>(
          builder.Configuration.GetSection("FileAttachmentSettings"));

            // Redis Configuration

            builder.Services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                var config = settings.GetConfiguration();
                return ConnectionMultiplexer.Connect(config);
            });
            // Http Client 
            builder.Services.AddHttpClient();

            // Redis service
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            // Serilog
            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                var connStr = context.Configuration.GetConnectionString("SerilogDB");

                var columnOptions = new ColumnOptions
                {
                    // Add your custom columns
                    AdditionalColumns =
                    [
                              new("UserId", SqlDbType.NVarChar, dataLength: 100),
                              new("Path", SqlDbType.NVarChar, dataLength: 500),
                              new("Method", SqlDbType.NVarChar, dataLength: 10),
                              new("StatusCode", SqlDbType.Int),
                              new("DurationMs", SqlDbType.Int),
                              new("RequestBody", SqlDbType.NVarChar, dataLength: -1),
                              new("ResponseBody", SqlDbType.NVarChar, dataLength: -1),
                              new("Headers", SqlDbType.NVarChar, dataLength: -1),
                              new("FirstError", SqlDbType.NVarChar, dataLength: -1),
                              new("Errors", SqlDbType.NVarChar, dataLength: -1),
                          ]
                };

                columnOptions.Store.Remove(StandardColumn.Properties);

                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.MSSqlServer(
                        connectionString: connStr,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "Logs",
                            AutoCreateSqlTable = true,
                            AutoCreateSqlDatabase = true
                        },
                        columnOptions: columnOptions,
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning
                    );
            });

            // Add services to the container.

            builder.Services.AddControllers();

            // Add Application Services to the container.
            builder.Services.AddApplicationServices(configuration);

            // Add Persistence Services to the container.
            builder.Services.AddPersistenceServices(configuration);
            builder.Services.AddCountryDashboardIntegrations();

            // Add Caching Services to the container.
            builder.Services.AddCaching(configuration);

            // Add Integrations to the container.
            builder.Services.AddAuthenticationServices(configuration);


            builder.Services.AddRequestContextServices();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(option =>
                {
                    option.Title = "Texas API";
                    option.Theme = ScalarTheme.BluePlanet;
                    option.ShowSidebar = true;
                    option.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<CustomRateLimitMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
