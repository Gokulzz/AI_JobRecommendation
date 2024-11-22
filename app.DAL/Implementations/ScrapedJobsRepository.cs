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
    public class ScrapedJobsRepository : GenericRepository<ScrapedJobs>, IScrapedJobsRepository, ICleanUpOldJobsRepository
    {
        public ScrapedJobsRepository(DataContext dataContext) : base(dataContext)
        {

        }
        public async Task<List<ScrapedJobs>> GetScrapedJobswithSkills()
        {
            var get_ScrapedJobs= await dataContext.ScrapedJobs.Include(x=>x.jobSkills).ToListAsync();
            return get_ScrapedJobs;

        }
        public async Task CleanUpOldJobsAsync()
        {
            await dataContext.Database.ExecuteSqlRawAsync("EXEC dbo.DeleteOldScrapedJobs");
        }
    }
}
