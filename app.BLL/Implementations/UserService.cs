using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.BLL.Exceptions;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using AutoMapper;
using FluentAssertions.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace app.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IEmailSenderService emailSenderService;
        private readonly IValidator<UserDTO> validator;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserService(IUnitofWork unitofWork, IValidator<UserDTO> validator, IConfiguration configuration, IEmailSenderService emailSenderService, IHttpContextAccessor httpContextAccessor)
        {
            this.unitofWork = unitofWork;
            this.validator = validator;
            this.configuration = configuration;
            this.emailSenderService = emailSenderService;
            this.httpContextAccessor = httpContextAccessor;
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
                var check_Email= await unitofWork.UserRepository.FindUserByEmail(userDTO.Email);
                if (check_Email!=null)
                {
                    throw new BadRequestException("User of this email already exists");
                }
                else
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
                        var message = new MessageDTO(new string[] { userDTO.Email }, "Please enter this verification token to register", add_User.verfificationToken.ToString());
                        await emailSenderService.SendEmailAsync(message);
                        return new ApiResponse(200, "Need to enter the verification token send to your email to complete the process of Registration", add_User);
                    }
                    else
                    {
                        throw new BadRequestException(validate_user.ToString());
                    }
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
        public async Task<ApiResponse> LoginUser(UserLoginDTO userLoginDTO)
        {
            var search_User = await unitofWork.UserRepository.FindUserByEmail(userLoginDTO.email);
            if (search_User == null)
            {
                throw new NotFoundException($"User of email= {userLoginDTO.email} could not be found");
            }
         
            else
            {
                List<Claim> Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, search_User.userName),
                new Claim(ClaimTypes.NameIdentifier, search_User.userId.ToString())
            };
                if (GetPasswordHash(userLoginDTO.password, search_User.passwordHash, search_User.passwordSalt))
                {
                    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT").GetSection("SecretKey").Value));
                    var SigningCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha512);
                    var token = new JwtSecurityToken(
                        issuer: configuration.GetSection("JWT").GetSection("ValidIssuer").Value,
                        audience: configuration.GetSection("JWT").GetSection("ValidAudience").Value,
                        expires: DateTime.Now.AddMinutes(5),
                        claims: Claims,
                        signingCredentials: SigningCredentials
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return new ApiResponse(201, "JWT Token generated successfully", tokenString);

                }
                else
                {
                    throw new NotAuthorizedException("Incorrect email or password");
                }
            }
            
          
        }
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var response = await unitofWork.UserRepository.FindUserByEmail(email);  
                if (response != null)
                {
                    return new OkObjectResult(new { exists = true });
                }
                return new NotFoundObjectResult(new { exits = false });
            }
            catch (NotFoundException)
            {
                return new NotFoundObjectResult(new { exits = false });
            }
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

        public void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordsalt)
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
        public Guid GetCurrentId()
        {
            //we are using httpcontext accessor as we are trying to access the http context inside  the service
            //we are getting the id of the authenticated user from the JWT token.
            var claimIdentity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (claimIdentity != null)
            {
                var userIdClaim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }
                throw new UnauthorizedAccessException("UserId not found");
            }
            else
            {
                throw new NotFoundException("UserId of this claim could not be found");
            }

        }
        public static Guid GenerateToken()
        {
            Guid token = Guid.NewGuid();
            return token;
        }

      
    }

}
