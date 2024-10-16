using app.BLL;
using app.BLL.DTO;
using app.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_JobRecommendation.Controllers
{
    [Authorize]
    public class UserResumeController : Controller
    {
        private readonly IResumeService resumeService;
        public UserResumeController(IResumeService resumeService)
        {
            this.resumeService = resumeService;
        }
        [HttpGet("GetUserResumeById")]
        public async Task<ApiResponse> GetUserResumeById(Guid id)
        {
            var resume = await resumeService.GetResume(id);
            return resume;
        }
        [HttpPost("AddResume")]
        public async Task<ApiResponse> AddResume( ResumeDTO resumeDTO)
        {
            var add_Resume= await resumeService.AddResume(resumeDTO);
            return add_Resume;
        }
    }
}
