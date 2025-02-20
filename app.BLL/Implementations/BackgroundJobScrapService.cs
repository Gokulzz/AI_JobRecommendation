using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace app.BLL.Implementations
{
    public class BackgroundJobScrapService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackgroundJobScrapService> _logger;
        private readonly IConnectionMultiplexer _redis;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // Job scrap interval time
        private const string ActiveUsersKey = "ActiveUsers"; // Redis key for active users

        public BackgroundJobScrapService(IServiceScopeFactory scopeFactory, ILogger<BackgroundJobScrapService> logger,IConnectionMultiplexer redis)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _redis = redis;
            _logger.LogInformation("BackgroundJobScrapService initialized.");
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background job scraping started.");
             while (!cancellationToken.IsCancellationRequested)
             {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = _redis.GetDatabase();

                    // Get all active user IDs from Redis (active users)
                    var activeUserIds = await db.SetMembersAsync(ActiveUsersKey);

                    foreach (var userId in activeUserIds)
                    {
                        var userGuid = Guid.Parse(userId);

                        try
                        {
                            var jobScrapService = scope.ServiceProvider.GetRequiredService<IJobScrapService>();
                            // Scrap jobs for each active user
                            await jobScrapService.ScrapJobs(userGuid);

                            // Log success for scraping
                            _logger.LogInformation("Jobs successfully scraped for user {UserId} at {Time}", userGuid, DateTimeOffset.UtcNow);
                        }
                        catch (Exception ex)
                        {
                            // Handle any errors in scraping
                            _logger.LogError(ex, "Error while scraping jobs for user {UserId}", userGuid);
                        }
                    }

                    // If no active users found, wait for the interval
                    if (activeUserIds.Length == 0)
                    {
                        _logger.LogInformation("No active users found, waiting for the next check.");
                        await Task.Delay(_interval, cancellationToken);
                    }
                }
            }

            _logger.LogInformation("Background job scraping stopped.");
        }
    }
}
