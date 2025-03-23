using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLMS.Web.Areas.FixturesAndResults.Controllers.ApiController
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class FixturesAndResultsApiController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
