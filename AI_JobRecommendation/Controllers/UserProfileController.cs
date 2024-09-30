using app.BLL.DTO;
using app.BLL.Implementations;
using app.BLL;
using app.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AI_JobRecommendation.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            this.userProfileService = userProfileService;
        }
        [HttpGet("GetUserProfileById")]
        public async Task<ApiResponse> GetUserProfileById(Guid id)
        {
            var getUser = await userProfileService.GetUserProfileById(id);
            return getUser;
        }
        [HttpGet("GetAllProfiles")]
        public async Task<ApiResponse> GetAllProfiles()
        {
            var user = await userProfileService.GetAllUserProfile();
            return user;
        }

        [HttpPost("AddUserProfile")]
        public async Task<ApiResponse> AddUserProfile([FromBody]UserProfileDTO userProfileDTO)
        {
            var getUsers = await userProfileService.AddUserProfile(userProfileDTO);
            return getUsers;
        }
        [HttpPut("UpdateUserProfile")]
        public async Task<ApiResponse> UpdateUserProfile(Guid Id, UserProfileDTO user)
        {
            var update_User = await userProfileService.UpdateUserProfile(Id, user);
            return update_User;
        }
        [HttpDelete("DeleteUserProfile")]
        public async Task<ApiResponse> DeleteUserProfile(Guid id)
        {
            var delete_User = await userProfileService.DeleteUserProfile(id);
            return delete_User;
        }

    }
}
