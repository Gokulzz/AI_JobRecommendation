using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;

namespace app.DAL.Repository
{
    public interface IJobRecommendationsRepository : IGenericRepository<JobRecommendation>
    {
        public Task<List<JobRecommendation>> GetJobRecommendation(Guid userId);
        public Task DeleteDuplicateJobs();
      
    }
}
