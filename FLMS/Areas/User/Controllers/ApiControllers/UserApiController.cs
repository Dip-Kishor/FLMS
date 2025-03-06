using CommonServices;
using Microsoft.AspNetCore.Mvc;
using FLMS.Services.User;
using FLMS.Services.User.ViewModels;

namespace FLMS.Web.Areas.User.Controllers.ApiControllers
{
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
    }
}
