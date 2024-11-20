using app.BLL;
using app.BLL.Implementations;
using app.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_JobRecommendation.Controllers
{
    [Authorize]
    public class JobRecommendationController : Controller
    {
        private readonly IJobRecommendationService jobRecommendationService;
        public JobRecommendationController(IJobRecommendationService jobRecommendationService)
        {
            this.jobRecommendationService = jobRecommendationService;
        }
        [HttpGet("GetJobRecommendation")]
        public async Task<ApiResponse> GetJobRecommendation()
        {
            var get_recommendedJobs= await jobRecommendationService.GetJobRecommendation();
            return get_recommendedJobs;
        }
        [HttpPost("AddJobRecommendation")]
        public async Task<ApiResponse> AddJobRecommendation()
        {
            var add_jobRecommendation= await jobRecommendationService.AddJobRecommendation();
            return add_jobRecommendation;
        }
    }
}
