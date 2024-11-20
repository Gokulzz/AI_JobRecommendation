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
    public class ResumeRepository :  GenericRepository<Resume>, IResumeRepository
    {
        public ResumeRepository(DataContext dataContext) : base(dataContext)
        {

        }
        public async Task<Guid?> GetResume(Guid userProfileId)
        {
            var resumeId = await dataContext.UserProfiles.Where(x => x.profileId == userProfileId).Select(x => x.resumeId).FirstOrDefaultAsync();
            return resumeId;
        }
    }
}
