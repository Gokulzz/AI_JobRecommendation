using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.BLL.Exceptions;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using AutoMapper;
using FluentValidation;

namespace app.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IMapper mapper;
        private readonly IValidator<UserDTO> validator;
        public UserService(IUnitofWork unitofWork, IMapper mapper, IValidator<UserDTO> validator)
        {
            this.unitofWork = unitofWork;
            this.mapper = mapper;
            this.validator = validator;
        }
        public async Task<ApiResponse> GetAllUser()
        {
            var users = await unitofWork.UserRepository.GetAllAsync();
            return new ApiResponse(200, "User Displayed successfully", users);

        }
        public async Task<ApiResponse> GetUser(Guid id)
        {
            var find_id = await unitofWork.UserRepository.GetAsync(id);
            return new ApiResponse(200, $"User of {id} displayed successfully", find_id);
        }
        public async Task<ApiResponse> AddUser(UserDTO userDTO)
        {
            try
            {


                var validate_user = validator.Validate(userDTO);
                if (validate_user.IsValid)
                {

                    CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordsalt);
                    var add_User = new User
                    {
                        userName = userDTO.userName,
                        Email = userDTO.Email,
                        passwordHash = passwordHash,
                        passwordSalt = passwordsalt,
                        verfificationToken = GenerateToken(),
                     

                    };
                    await unitofWork.UserRepository.PostAsync(add_User);
                    await unitofWork.Save();
                    //var message = new MessageDTO(new string[] { userDTO.Email }, "Please enter this verification token to register", add_User.VerificationToken.ToString());
                    //await emailSenderService.SendEmailAsync(message);
                    return new ApiResponse(200, "Need to enter the verification token send to your email to complete the process of Registration", add_User);
                }
                else
                {
                    throw new BadRequestException(validate_user.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }


        }
        public async Task<ApiResponse> DeleteUser(Guid id)
        {
            var find_User = await unitofWork.UserRepository.DeleteAsync(id);
            if (find_User == null)
            {
                throw new NotFoundException($"User of this {id} could not be found");
            }
            return new ApiResponse(200, $"User of id= {id} deleted successfully", find_User);
        }
        public async Task<ApiResponse> UpdateUser(Guid id, UserDTO userDTO)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResponse> VerifyUser(Guid Token)
        {

            var user = await unitofWork.UserRepository.VerifyUser(Token);
            if (user == null)
            {
                throw new BadRequestException("Incorrect verfication token");
            }
            user.verfiedAt = DateTime.Now;
            await unitofWork.Save();
            return new ApiResponse(200, "User Verified Successfully", user);
        }

        public static void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
            }

        }
        public static bool GetPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return computeHash.SequenceEqual(PasswordHash);
            }
        }

        public static Guid GenerateToken()
        {
            Guid token = Guid.NewGuid();
            return token;
        }

        public Task<ApiResponse> GetUserByEmail(string Email)
        {
            throw new NotImplementedException();
        }

        
        public Task<ApiResponse> LoginUser(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }
    }

}
