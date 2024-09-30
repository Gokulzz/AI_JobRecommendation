using app.BLL;
using app.BLL.DTO;
using app.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI_JobRecommendation.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly IUserService userService;
        public UserLoginController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost("UserLogin")]
        public async Task<ApiResponse> UserLogin([FromBody] UserLoginDTO userLogin)
        {
            var user_login = await userService.LoginUser(userLogin);
            return user_login;
        }
    }
}
