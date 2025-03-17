using CommonServices;
using Microsoft.AspNetCore.Mvc;
using FLMS.Services.User;
using FLMS.Services.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FLMS.Services.TokenValidation;

namespace FLMS.Web.Areas.User.Controllers.ApiControllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly SUser _user;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public UserApiController(SUser user, ITokenBlacklistService tokenBlacklistService)
        {
            _user = user;
            _tokenBlacklistService = tokenBlacklistService;
        }
        [HttpPost("createAccount")]
        public ServiceResult<UserVM> CreateAccount(UserVM vm)
        {
            var result = _user.CreateAccount(vm);
            return new ServiceResult<UserVM>()
            {
                Data = result.Data,
                Message = result.Message,
                Status = result.Status,
            };
        }
        [HttpPost("login")]
        public ServiceResult<LoginResponse> Login(LoginVM vm)
        {
            var result = _user.Login(vm, Response);
            return new ServiceResult<LoginResponse>
            {
                Data = result.Data,
                Message = result.Message,
                Status = result.Status,
            };
        }
        [HttpGet("logout")]
        public async Task<ServiceResult<string>> Logout()
        {
            var token = Request.Cookies["accessToken"];
            if (!string.IsNullOrEmpty(token))
            {
                await _tokenBlacklistService.RevokeToken(token);
            }

            // Expire the cookie properly
            Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(-1)  // Force immediate expiration
            });

            return new ServiceResult<string>
            {
                Data = null,
                Message = "Logout successful",
                Status = ResultStatus.Ok
            };
        }



    }
}
