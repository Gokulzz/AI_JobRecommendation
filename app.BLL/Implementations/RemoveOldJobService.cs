using System;
using System.Threading;
using System.Threading.Tasks;
using app.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace app.BLL.Implementations
{
    public class RemoveOldJobService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RemoveOldJobService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);

        public RemoveOldJobService(IServiceScopeFactory scopeFactory, ILogger<RemoveOldJobService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //using scope factory creating a scoped service...
                //since ICleanUpOldJobRepository is a scoped service which is being consumed by singleton class
                //.net doesnot allow that
                //so we have to create a scoped service for each run that uses IcleanupoldjobsRepository.
                await Task.Delay(_interval, cancellationToken);  
                using (var scope = _scopeFactory.CreateScope())
                {
                    var cleanupRepositories = scope.ServiceProvider.GetRequiredService<IEnumerable<ICleanUpOldJobsRepository>>();
                    foreach (var cleanUpRepository in cleanupRepositories)
                    {
                        try
                        {
                            await cleanUpRepository.CleanUpOldJobsAsync();
                            _logger.LogInformation("Old jobs cleaned up at: {time}", DateTimeOffset.Now);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error occurred while cleaning up old jobs");
                        }
                    }
                }
            }
        }
    }
}
