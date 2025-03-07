﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;

namespace app.DAL.Repository
{
    public interface IJobSkillRepository : IGenericRepository<JobSkill>
    {
        public Task<List<JobSkill>> GetAllJobSkills();
    }
}
