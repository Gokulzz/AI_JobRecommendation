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
    public class JobPreferencesRepository : GenericRepository<JobPreferences>, IJobPreferencesRepository    
    {
        public JobPreferencesRepository(DataContext dataContext) : base(dataContext)
        {

        }
        public async Task<JobPreferences?> GetTitleAndLocation(Guid userId)
        {
            var getData = await dataContext.JobPreferences.FirstOrDefaultAsync(x => x.UserId == userId);

            if (getData == null)
            {
                throw new KeyNotFoundException("Job preferences not found for the specified user ID.");
            }

            return new JobPreferences
            {
                PreferredJobTitle = getData.PreferredJobTitle,
                PreferredLocation = getData.PreferredLocation
            };
        }

    }
}
