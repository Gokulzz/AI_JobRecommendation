using app.BLL;
using app.BLL.DTO;
using app.BLL.Implementations;
using app.BLL.Services;
using app.DAL.Implementations;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AI_JobRecommendation.Controllers
{
    [Authorize]
    public class JobScrapController : Controller
    {
        
        private readonly IJobScrapService _jobScrapService;
        public JobScrapController(IJobScrapService jobScrapService)
        {
            _jobScrapService= jobScrapService;
        }
        [HttpGet("GetScrapedJobwithSkill")]
        public async Task<ApiResponse> GetScrapedJobwithSkill()
        {
            var result = await _jobScrapService.GetScrapJobs();
            return result;
        }
        //[HttpPost("ScrapJob")]
        //public async Task<IActionResult> ScrapJob()
        //{
        //    var result = await _jobScrapService.ScrapJobs();
        //    return Ok(result);

        //}
    }
}
