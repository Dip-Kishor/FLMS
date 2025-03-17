using CommonServices;
using FLMS.Services.PlayersRegistration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FLMS.Web.Areas.PlayersRegistration.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerRegistrationApiController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost("register")]
        public async Task<ServiceResult<PlayersRegistrationVM>> Register([FromForm] PlayersRegistrationVM vm)
        {
            if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", vm.ImageUrl.FileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.ImageUrl.CopyToAsync(stream);
                }

                vm.ImageUrl = null; 
            }

            return new ServiceResult<PlayersRegistrationVM>()
            {
                Data = vm,
                Message = "Successfully registered for this season",
                Status = ResultStatus.Ok
            };
        }

    }
}
