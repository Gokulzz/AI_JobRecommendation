﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Repository
{
    public interface ICleanUpOldJobsRepository
    {
        public Task CleanUpOldJobsAsync();
    }
}
