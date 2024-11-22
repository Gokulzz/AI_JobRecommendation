using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Data;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace app.DAL.Implementations
{
    public class JobSkillRepository : GenericRepository<JobSkill>, IJobSkillRepository
    {
        public JobSkillRepository(DataContext dataContext) : base(dataContext)
        {
         

        }
        public async Task<List<JobSkill>> GetAllJobSkills()
        {
            var get_AllJobSkills = await dataContext.JobSkills.ToListAsync();
            return get_AllJobSkills;
        }

       
    }
}
