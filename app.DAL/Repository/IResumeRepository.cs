﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;

namespace app.DAL.Repository
{
    public interface IResumeRepository : IGenericRepository<Resume>
    {
        public Task<Guid?> GetResume(Guid userProfileID);
    }
}
