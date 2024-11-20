using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Data;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace app.DAL.Implementations
{
    public class JobRecommendationsRepository : GenericRepository<JobRecommendation>, IJobRecommendationsRepository, ICleanUpOldJobsRepository
    { 
        public JobRecommendationsRepository(DataContext dataContext): base(dataContext)
        {

        }
        public async Task<List<JobRecommendation>> GetJobRecommendation(Guid userId)
        {
            var get_Jobs = await dataContext.JobRecommendations.Where(x => x.UserId == userId).Include(x => x.ScrapedJob).ToListAsync();
            return get_Jobs;
        }
        public async Task CleanUpOldJobsAsync()
        {
            await dataContext.Database.ExecuteSqlRawAsync("EXEC dbo.DeleteOldJobs");
        }

    }
}
