﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Data;
using app.DAL.Models;
using app.DAL.Repository;

namespace app.DAL.Implementations
{
    public class ResumeRepository :  GenericRepository<Resume>, IResumeRepository
    {
        public ResumeRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
}
