using Microsoft.AspNetCore.Mvc;

namespace FLMS.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("...");
        }
    }
}
