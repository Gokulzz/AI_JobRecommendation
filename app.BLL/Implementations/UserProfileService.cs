using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.BLL.Exceptions;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using MimeKit.Encodings;

namespace app.BLL.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IUserService userService;
        public UserProfileService(IUnitofWork unitofWork, IUserService userService)
        {
            this.unitofWork = unitofWork;
            this.userService = userService;
        }
        public async Task<ApiResponse> GetAllUserProfile()
        {
            var userProfiles = await unitofWork.UserProfileRepository.GetAllAsync();
            if (!userProfiles.Any())
            {
                throw new NotFoundException("user profile could not be  found");
            }
            return new ApiResponse(200, "User Profiles displayed successfully", userProfiles);
        }
        public async Task<ApiResponse> GetUserProfileById(Guid profileId)
        {
            var userProfileId = await unitofWork.UserProfileRepository.GetAsync(profileId);
            if (userProfileId.Equals(Guid.Empty))
            {
                throw new NotFoundException($"User of this {profileId} does not exist");
            }
            return new ApiResponse(200, $"User of this {profileId} does not exist.", userProfileId);
        }
        public async Task<ApiResponse> AddUserProfile(UserProfileDTO profileDTO)
        {
            try
            {
                var addUserProfile = new UserProfile()
                {
                    userId = userService.GetCurrentId(),
                    firstName = profileDTO.firstName,
                    lastName = profileDTO.lastName,
                    phoneNumber = profileDTO.phoneNumber,
                    experienceLevel = profileDTO.experienceLevel,
                    currentCompany = profileDTO.currentCompany,
                    Address = profileDTO.Address,
                    ProfileCreatedAt = DateTime.Now
                };
                await unitofWork.UserProfileRepository.PostAsync(addUserProfile);
                await unitofWork.Save();
                return new ApiResponse(200, "User added successfully", addUserProfile);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateUserProfile(Guid id, UserProfileDTO userProfileDTO)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResponse> DeleteUserProfile(Guid id)
        {
            try
            {
                var deleteUser = await userService.DeleteUser(id);
                return new ApiResponse(200, "User deleted successfully", deleteUser);
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }
    }
}

