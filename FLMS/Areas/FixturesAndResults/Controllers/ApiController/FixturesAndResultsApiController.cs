using CommonServices;
using FLMS.Services.FIxturesAndResults;
using FLMS.Services.PlayersRegistration.ViewModels;
using FLMS.Services.PlayersRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FLMS.Services.FIxturesAndResults.ViewModels;
using FLMS.Data.SP_Result;
using FLMS.Models.FixturesAndResults;

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

        ///Alll users
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
        [HttpPost("getFixtures")]
        public ServiceResult<List<FixturesAndResultsVM>> GetFixtures(int seasonId )
        {
            var result = _fixtureServices.GetFixtures(seasonId);
            if (result.Status == ResultStatus.Ok)
            {
                result.Message = $"Successfully Fetched.";
            }
            return result;
        }

        //admin and superadmin
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
        [HttpPost("updateFixtures")]
        public ServiceResult<FixturesAndResultsVM> UpdateFixture(FixturesAndResultsVM vm)
        {
            if (vm == null)
            {
                return new ServiceResult<FixturesAndResultsVM>()
                {
                    Data = null,
                    Message = "Empty vm",
                    Status = ResultStatus.processError,
                };
            }
            var result = _fixtureServices.UpdateFixture(vm);
            return new ServiceResult<FixturesAndResultsVM>()
            {
                Data = vm,
                Message = "Successfully updated fixtures data",
                Status = ResultStatus.Ok,
            };
        }
        [HttpPost("getTable")]
        public async Task<ServiceResult<List<ReportVm>>> GetTable(int seasonId)
        {
            var result = await _fixtureServices.GetTableAsync(seasonId); // Await the async method

            if (result.Status != ResultStatus.Ok || result.Data == null)
            {
                return new ServiceResult<List<ReportVm>>()
                {
                    Data = new List<ReportVm>(),
                    Message = "Failed to fetch data",
                    Status = ResultStatus.processError
                };
            }

            var data = result.Data.Select(r => new ReportVm
            {
                Rank = (int)r.Rank,
                PlayerId = r.PlayerId,
                PlayerName = r.PlayerName,
                ImageUrl = r.ImageUrl,
                MatchesPlayed = r.MatchesPlayed,
                Wins = r.Wins,
                Draws = r.Draws,
                Losses = r.Losses,
                GF = r.GF,
                GA = r.GA,
                GD = r.GD,
                Points = r.Points,
                Group = (GroupType)r.Group,
            }).ToList();

            return new ServiceResult<List<ReportVm>>()
            {
                Data = data,
                Message = "Success",
                Status = ResultStatus.Ok
            };
        }

        public class ReportVm
        {
            public GroupType Group { get; set; }
            public int Rank { get; set; }
            public int PlayerId { get; set; }
            public string PlayerName { get; set; }
            public string ImageUrl { get; set; }
            public int MatchesPlayed { get; set; }
            public int Wins { get; set; }
            public int Draws { get; set; }
            public int Losses { get; set; }
            public int GF { get; set; }
            public int GD { get; set; }
            public int GA { get; set; }
            public int Points { get; set; }
        }
    }
}
