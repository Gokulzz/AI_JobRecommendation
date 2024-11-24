using app.BLL;
using app.BLL.DTO;
using app.BLL.Implementations;
using app.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_JobRecommendation.Controllers
{
    public class PasswordResetController : Controller
    {
        private readonly IPasswordResetService passwordResetService;
        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            this.passwordResetService = passwordResetService;
        }
        [HttpPost("PasswordResetToken")]
        public async Task<ApiResponse> PasswordResetToken(string email)
        {
            var get_Token= await passwordResetService.ResetPasswordToken(email);
            return get_Token;
        }
       
        [HttpPost("ChangePasswordWithToken")]
        public async Task<ApiResponse> ChangePasswordWithToken([FromBody] PasswordResetDTO passwordDTO)
        {
            var change_Password= await passwordResetService.ResetPasswordWithToken(passwordDTO);
            return change_Password;
        }

    }
}
