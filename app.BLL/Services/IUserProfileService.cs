using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;

namespace app.BLL.Services
{
    public interface IUserProfileService
    {
        public Task<ApiResponse> GetAllUserProfile();
        public Task<ApiResponse> GetUserProfileById(Guid Id);
        public Task<ApiResponse> AddUserProfile(UserProfileDTO userDTO);
        public Task<ApiResponse> UpdateUserProfile(Guid Id, UserProfileDTO userDTO);
        public Task<ApiResponse> DeleteUserProfile(Guid Id);
  
    }
}
