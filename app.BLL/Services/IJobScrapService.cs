using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.BLL.Services
{
    public interface IJobScrapService
    {
        public Task<ApiResponse> ScrapJobs(JobPreferencesDTO jobPreferencesDTO);
    }
}
