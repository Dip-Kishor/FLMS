using CommonServices;
using FLMS.Services.FIxturesAndResults;
using FLMS.Services.PlayersRegistration.ViewModels;
using FLMS.Services.PlayersRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FLMS.Services.FIxturesAndResults.ViewModels;

namespace FLMS.Web.Areas.FixturesAndResults.Controllers.ApiController
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class FixturesAndResultsApiController : ControllerBase
    {
        private readonly SFixturesAndResults _fixtureServices;
        public FixturesAndResultsApiController(SFixturesAndResults fixturesAndResults)
        {
            _fixtureServices = fixturesAndResults;
        }
        [HttpPost("getAllPlayers")]
        public ServiceResult<ListOfPlayers> GetAllPlayers(int seasonId)
        {
            var result = _fixtureServices.GetAllPlayers(seasonId);

            if (result.Status == ResultStatus.Ok)
            {
                result.Message = $"Successfully retrieved {result.Data.playersList.Count} players.";
            }

            return result;
        }
        [HttpPost("createFixtures")]
        public ServiceResult<FixtureAndResultCreationVM> CreateFixture(FixtureAndResultCreationVM model)
        {
            var result = _fixtureServices.CreateFixture(model);
            if(result.Status == ResultStatus.Ok) 
            {
                result.Message = $"Successfully created  fixtures.";
            }
            return result;
        }
    }
}
