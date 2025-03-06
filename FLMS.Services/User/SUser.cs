using CommonServices;
using FLMS.Data;
using FLMS.Models.User;
using FLMS.Services.User.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.User
{
    public class SUser
    {
        private readonly FLMSContext _context;
        public SUser(FLMSContext context)
        {
            _context = context;
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
        
    }
}
