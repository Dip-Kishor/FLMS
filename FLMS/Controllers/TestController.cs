using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLMS.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TestController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Content("Unauthorized user");
        }
        [Authorize(Policy = "SuperAdmin")]
        [HttpGet("SuperAdmin")]
        public IActionResult SuperAdmin()
        {
            return Content("Authorized Super Admin");
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return Content("Authorized Admin");
        }
        [Authorize(Policy = "User")]
        [HttpGet("User")]
        public IActionResult User()
        {
            return Content("Authorized User");
        }
        [Authorize(Policy = "All")]
        [HttpGet("AllUsers")]
        public IActionResult AllUsers()
        {
            return Content("Authorized for all users");
        }
        [Authorize(Policy = "SuperAdminAndAdmin")]
        [HttpGet("SuperAdminAndAdmin")]
        public IActionResult SuperAdminAndAdmin()
        {
            return Content("Authorized for super admin and admin");
        }
    }
}
