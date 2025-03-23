using CommonServices;
using FLMS.Services.PlayersRegistration.ViewModels;
using FLMS.Services.PlayersRegistration;
using Microsoft.AspNetCore.Mvc;
using FLMS.Services;
using FLMS.Web.Areas.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FLMS.Web.Areas.ApiController
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonApiController : ControllerBase
    {
        private readonly SSeason _season;
        public SeasonApiController(SSeason season)
        {
            _season = season;
        }
        [HttpPost("getAllSeasons")]
        public ServiceResult<SeasonVM> GetAllSeasons()
        {
            var result = _season.GetAllSeasons();

            if (result.Status == ResultStatus.Ok)
            {
                result.Message = $"Successfully retrieved {result.Data.seasonVMs.Count} players.";
            }

            return result;
        }
    }
}
