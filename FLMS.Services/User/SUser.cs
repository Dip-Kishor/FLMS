using CommonServices;
using FLMS.Data;
using FLMS.Models.User;
using FLMS.Services.User.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.User
{
    public class SUser
    {
        private readonly FLMSContext _context;
        private readonly IConfiguration _configuration;

        public SUser(FLMSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public ServiceResult<UserVM> CreateAccount(UserVM user)
        {
            if (user == null)
            {
                return new ServiceResult<UserVM>
                {
                    Data = null,
                    Message = "Model is null",
                    Status = ResultStatus.processError
                };
            }
            var data = new EUser()
            {
                Username = user.username,
                Email = user.email,
                Password = user.password, 
                ConfirmPassword = user.confirmPassword,
            };
            var existingUser = _context.Users.FirstOrDefault(x => x.Email == data.Email);
            if (existingUser != null)
            {
                return new ServiceResult<UserVM>
                {
                    Data = null,
                    Message = "User already exists",
                    Status = ResultStatus.processError
                };
            }
            if (!_context.Users.Any(x => x.RoleId == 1)) // No SuperAdmin exists
            {
                data.RoleId = 1;
            }
            else if (!_context.Users.Any(x => x.RoleId == 2)) // No Admin exists
            {
                data.RoleId = 2;
            }
            else
            {
                data.RoleId = 3; // Assign User role
            }
            _context.Users.Add(data);
            _context.SaveChanges();

            return new ServiceResult<UserVM>
            {
                Data = null,
                Message = "User registered successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<string> Login(LoginVM vm)
        {
            if (vm == null)
            {
                return new ServiceResult<string>
                {
                    Data = null,
                    Message = "Input is empty",
                    Status = ResultStatus.processError
                };
            }
            var getUser = _context.Users.FirstOrDefault(x => x.Email == vm.email);
            if (getUser == null)
            {
                return new ServiceResult<string>
                {
                    Data = null,
                    Message = "User not found",
                    Status = ResultStatus.processError
                };
            }
            if(getUser.Password != vm.password)
            {
                return new ServiceResult<string>
                {
                    Data = null,
                    Message = "Inavalid username or password",
                    Status = ResultStatus.processError
                };
            }
            var userRole = _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == vm.email);
            var roleName = userRole.Role.RoleName;
            var userSession = new UserSession(getUser.UserId, getUser.Username, getUser.Email, roleName);
            string accessToken = GenerateAccessToken(userSession);

            return new ServiceResult<string>
            {
                Data = accessToken,
                Message = "Logged in succesfully",
                Status = ResultStatus.Ok
            };

        }
        //Token generation method
        private string GenerateAccessToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AccessToken:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var accessToken = new JwtSecurityToken
            (
                issuer: _configuration["AccessToken:Issuer"],
                audience: _configuration["AccessToken:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}
