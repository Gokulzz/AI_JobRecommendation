using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;

namespace app.BLL.Services
{
    public interface IPasswordResetService
    {
        public Task<ApiResponse> ResetPasswordToken(string email);
        public Task<ApiResponse> ResetPasswordWithToken(PasswordResetDTO password);
    }
}
