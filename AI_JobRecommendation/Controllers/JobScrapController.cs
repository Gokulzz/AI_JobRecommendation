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

        [HttpPost("ScrapJob")]
        public async Task<IActionResult> ScrapJob([FromBody] JobPreferencesDTO jobPreferencesDTO)
        {
            var result= await _jobScrapService.ScrapJobs(jobPreferencesDTO);
            return Ok(result);

        }
    }
}
