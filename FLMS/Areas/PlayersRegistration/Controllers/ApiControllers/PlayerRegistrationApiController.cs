using CommonServices;
using FLMS.Services.PlayersRegistration;
using FLMS.Services.PlayersRegistration.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLMS.Web.Areas.PlayersRegistration.Controllers.ApiControllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerRegistrationApiController : ControllerBase
    {
        private readonly SPlayerRegistration _playerRegistration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PlayerRegistrationApiController(SPlayerRegistration playerRegistration, IWebHostEnvironment webHostingEnvironment)
        {
            _playerRegistration = playerRegistration;
            _webHostEnvironment = webHostingEnvironment;

        }
        [HttpPost("register")]
        public ServiceResult<PlayersRegistrationVM>Create([FromForm]PlayersRegistrationVM vm,IFormFile? imageFile, IFormFile? teamImageFile)
        {
            string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            string filePath = Path.Combine(uploads, imageFile.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
                vm.imageUrl = "/uploads/" + imageFile.FileName;
            }
            if (teamImageFile != null) 
            {
                string teamFilePath = Path.Combine(uploads, teamImageFile.FileName);
                using (Stream teamFileStream = new FileStream(teamFilePath, FileMode.Create))
                {
                    teamImageFile.CopyTo(teamFileStream);
                    vm.teamImageUrl = "/uploads/" + teamImageFile.FileName;
                }
            }
            var result = _playerRegistration.RegisterPlayer(vm, HttpContext);
            return new ServiceResult<PlayersRegistrationVM>()
            {
                Data = vm,
                Message = result.Message,
                Status = result.Status,
            };
        }
        [HttpPost("getAllPlayers")]
        public ServiceResult<ListOfPlayers> GetAllPlayers(int seasonId)
        {
            var result = _playerRegistration.GetAllPlayers(seasonId);

            if (result.Status == ResultStatus.Ok)
            {
                result.Message = $"Successfully retrieved {result.Data.playersList.Count} players.";
            }

            return result;
        }
        [HttpPost("GetPlayersForAdmin")]
        public ServiceResult<ListOfPlayers> GetPlayersForAdmin(int seasonId)
        {
            var result = _playerRegistration.GetPlayersForAdmin(seasonId);

            if (result.Status == ResultStatus.Ok)
            {
                result.Message = $"Successfully retrieved {result.Data.playersList.Count} players.";
            }

            return result;
        }
        [HttpPost("ApprovePlayers")]
        public ServiceResult<int> ApprovePlayers (int playerId)
        {
            var result = _playerRegistration.Approve(playerId);
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = result.Message,
                Status = result.Status,
            };
        }
    }
}
