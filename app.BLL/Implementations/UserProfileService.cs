﻿using System;
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
        public async Task<ApiResponse> GetUserProfileByUserId()
        {
            var userId= userService.GetCurrentId();
            var userProfileId= await unitofWork.UserProfileRepository.GetUserProfileId(userId);
            if(userProfileId==Guid.Empty)
            {
                throw new NotFoundException($"User of this {userProfileId} does not exist");
            }
            return new ApiResponse(200, $"USER__PROFILE_ID of this {userProfileId} returned successfully", userProfileId);
        }
        public async Task<ApiResponse> GetUserProfileById(Guid profileId)
        {
            var userProfileId = await unitofWork.UserProfileRepository.GetAsync(profileId);
            if (userProfileId.Equals(Guid.Empty))
            {
                throw new NotFoundException($"User of this {profileId} does not exist");
            }
            return new ApiResponse(200, $"User of this {profileId} returned successfully.", userProfileId);
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
                var addJobPreferences = new JobPreferences()
                {
                    UserId = userService.GetCurrentId(),
                    PreferredJobTitle = profileDTO.PreferredJobTitle,
                    PreferredLocation = profileDTO.PreferredLocation
                };
                await unitofWork.UserProfileRepository.PostAsync(addUserProfile);
                await unitofWork.JobPreferencesRepository.PostAsync(addJobPreferences);
                await unitofWork.Save();
                return new ApiResponse(200, "User added successfully", addUserProfile);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<ApiResponse> GetJobPreferences()
        {
            try
            {
                var userId=  userService.GetCurrentId();
                var get_Data= await unitofWork.JobPreferencesRepository.GetTitleAndLocation(userId);
                return new ApiResponse(200, "Title and location returned successfully", get_Data);
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

