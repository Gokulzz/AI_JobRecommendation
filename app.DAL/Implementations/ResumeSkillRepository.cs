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
    public class ResumeSkillRepository : GenericRepository<ResumeSkill>, IResumeSkillRepository
    {
        public ResumeSkillRepository(DataContext dataContext) : base(dataContext)
        {

        }
        public async Task<List<string>> GetResumeSkills(Guid? resumeId)
        {
            var get_Skills = await dataContext.ResumeSkills.Where(x => x.ResumeId == resumeId).Select(x => x.SkillName).ToListAsync();
            return get_Skills;
        }
    }
}
