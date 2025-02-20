using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace app.DAL.Implementations
{
    public class RemoveDuplicateJobService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RemoveDuplicateJobService> _logger;
        //execute command after every 30 minutes
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);
        public RemoveDuplicateJobService(IServiceScopeFactory scopeFactory, ILogger<RemoveDuplicateJobService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
          
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);
                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var deleteRecommendedJobs = scope.ServiceProvider.GetRequiredService<IJobRecommendationsRepository>();
                        await deleteRecommendedJobs.DeleteDuplicateJobs();
                        _logger.LogInformation("duplicate recommended jobs deleted successfully at {time}", DateTimeOffset.UtcNow);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        
                    }
                }
            }
        }
    }
}
