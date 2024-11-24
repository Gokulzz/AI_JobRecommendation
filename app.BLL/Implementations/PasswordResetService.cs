using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.BLL.Exceptions;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using FluentValidation;
using MimeKit.Cryptography;

namespace app.BLL.Implementations
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IUserService userService;
        private readonly IEmailSenderService emailSenderService;
        private readonly IValidator<PasswordResetDTO> validator;
        public PasswordResetService(IUnitofWork unitofWork, IUserService userService, IEmailSenderService emailSenderService, IValidator<PasswordResetDTO> validator)
        {
            this.unitofWork = unitofWork;
            this.userService = userService;
            this.emailSenderService = emailSenderService;
            this.validator = validator;
        }   
        public async Task<ApiResponse> ResetPasswordToken(string email)
        {
            var find_Email = await unitofWork.UserRepository.FindUserByEmail(email);
            if(find_Email == null)
            {
                throw new NotFoundException($"Could not find user of this email {email}");
            }
            else
            {
                var add_Data = new PasswordReset()
                {
                    userId = find_Email.userId,
                    resetToken = Guid.NewGuid(),
                    tokencreatedAt = DateTime.UtcNow,
                    tokenExpiresAt = DateTime.UtcNow.AddMinutes(5),
                    used=false
                };
                await unitofWork.PasswordResetRepository.PostAsync(add_Data);
                await unitofWork.Save();
                var message = new MessageDTO(new string[] { email }, "Please enter this token to reset your password", add_Data.resetToken.ToString());
                await emailSenderService.SendEmailAsync(message);
                return new ApiResponse(200, "Enter this token to reset your password", add_Data);
            }
        }
        public async Task<ApiResponse> ResetPasswordWithToken(PasswordResetDTO passwordResetDTO)
        {
            if (string.IsNullOrWhiteSpace(passwordResetDTO.Token))
            {
                throw new BadRequestException("Token cannot be null or empty");
            }

            if (!Guid.TryParse(passwordResetDTO.Token, out var parsedToken))
            {
                throw new BadRequestException("Invalid token format");
            }

            // Validate the token
            var resetRequest = await unitofWork.PasswordResetRepository.CheckToken(parsedToken);
            if (resetRequest == null || resetRequest.used == true || resetRequest.tokenExpiresAt <= DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid or expired token");
            }

            // Fetch the associated user
            var getUser = await unitofWork.UserRepository.GetAsync(resetRequest.userId);
            if (getUser == null)
            {
                throw new NotFoundException("User not found");
            }

            // Validate the new password
            var validatePassword = validator.Validate(passwordResetDTO);
            if (!validatePassword.IsValid)
            {
                throw new BadRequestException(validatePassword.ToString());
            }

            // Update the user's password
            userService.CreatePasswordHash(passwordResetDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            getUser.passwordHash = passwordHash;
            getUser.passwordSalt = passwordSalt;

            // Mark the token as used
            resetRequest.used = true;

            await unitofWork.Save();

            return new ApiResponse(200, "Password changed successfully", getUser);
        }

    }
}

