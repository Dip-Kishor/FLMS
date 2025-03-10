using CommonServices;
using Microsoft.AspNetCore.Mvc;
using FLMS.Services.User;
using FLMS.Services.User.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FLMS.Web.Areas.User.Controllers.ApiControllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly SUser _user;
        public UserApiController(SUser user)
        {
            _user = user;
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
        [HttpPost("logout")]
        public ServiceResult<string> Logout()
        {
            Response.Cookies.Delete("accessToken");
            return new ServiceResult<string>()
            {
                Data = null,
                Message = "Logout successful",
                Status = ResultStatus.Ok
            };
        }

    }
}
