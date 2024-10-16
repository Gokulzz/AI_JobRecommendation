using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;

namespace app.DAL.Repository
{
    public interface IJobPreferencesRepository : IGenericRepository<JobPreferences>
    {
        public Task<JobPreferences?> GetTitleAndLocation(Guid userId);
    }
}
