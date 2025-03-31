using CommonServices;
using FLMS.Data;
using FLMS.Models.PlayerRegistration;
using FLMS.Services.PlayersRegistration.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.PlayersRegistration
{
    public class SPlayerRegistration
    {
        private readonly FLMSContext _context;
        public SPlayerRegistration(FLMSContext context)
        {
            _context = context; 
        }
        public ServiceResult<PlayersRegistrationVM> RegisterPlayer(PlayersRegistrationVM vm,HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["accessToken"];
            if (token == null)
            {
                return new ServiceResult<PlayersRegistrationVM>
                {
                    Data = null,
                    Message = "Session expired please login again",
                    Status = ResultStatus.processError
                };
            }
            var userId = GetUserIdFromToken(token);
            var currSeason = _context.Seasons.Where(x=>x.IsCurrentSeason == true).FirstOrDefault();
            if(currSeason == null)
            {
                return new ServiceResult<PlayersRegistrationVM>()
                {
                    Data = null,
                    Message = "Registration is not available right now, Please come back later",
                    Status = ResultStatus.processError
                };
            }
            var seasonComplete = _context.Seasons.Where(x => x.IsSeasonComplete == false).FirstOrDefault();
            if(seasonComplete == null)
            {
                return new ServiceResult<PlayersRegistrationVM>()
                {
                    Data = null,
                    Message = "Registration is not available right now, Please come back later",
                    Status = ResultStatus.processError
                };
            }
            var existingUser = _context.RegisteredPlayers.Where(x => x.UserId == userId && x.SeasonId == currSeason.Id).FirstOrDefault();
            if (existingUser != null)
            {
                return new ServiceResult<PlayersRegistrationVM>()
                {
                    Data = null,
                    Message = "Already registered by the same user. Please login with different account",
                    Status = ResultStatus.processError
                };
            }
            var players = new ERegisteredPlayers()
            {
                SeasonId = currSeason.Id,
                UserId = userId ?? 0,
                Name = vm.name,
                Email = vm.email,
                Gender = vm.gender,
                EFootballId = vm.eFootballId,
                InGameName = vm.inGameName,
                ImageUrl = vm.imageUrl,
                IsApproved = vm.isApproved,
                TeamImageUrl = vm.teamImageUrl,
            };
            _context.RegisteredPlayers.Add(players);
            _context.SaveChanges();
            return new ServiceResult<PlayersRegistrationVM>()
            {
                Data = vm,
                Message = "Registered successfully for the tournament, Please check admin verification in your mail",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ListOfPlayers> GetAllPlayers(int seasonId)
        {
            var players = _context.RegisteredPlayers.Where(x=>x.IsApproved == true && x.SeasonId == seasonId)
                .Select(p => new PlayersRegistrationVM
                {
                    id = p.Id,
                    seasonId = p.SeasonId,
                    name = p.Name,
                    email = p.Email,
                    gender = p.Gender,
                    eFootballId = "ASBB-000-000",
                    inGameName = p.InGameName,
                    imageUrl = p.ImageUrl,
                    isApproved = p.IsApproved,
                    teamImageUrl =p.TeamImageUrl,
                })
                .ToList();

            if (players == null)  // Handle empty results properly
            {
                return new ServiceResult<ListOfPlayers>()
                {
                    Data = null,
                    Message = "No registered players found for this season.",
                    Status = ResultStatus.processError
                };
            }

            return new ServiceResult<ListOfPlayers>()
            {
                Data = new ListOfPlayers { playersList = players },  // Wrap inside ListOfPlayers
                Message = "Successfully retrieved the registered player data.",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ListOfPlayers> GetPlayersForAdmin(int seasonId)
        {
            var players = _context.RegisteredPlayers.Where(x => x.SeasonId == seasonId)
                .Select(p => new PlayersRegistrationVM
                {
                    id = p.Id,
                    seasonId = p.SeasonId,
                    name = p.Name,
                    email = p.Email,
                    gender = p.Gender,
                    eFootballId = "ASBB-000-000",
                    inGameName = p.InGameName,
                    imageUrl = p.ImageUrl,
                    isApproved = p.IsApproved,
                    teamImageUrl = p.TeamImageUrl,
                })
                .ToList();

            if (players == null)  // Handle empty results properly
            {
                return new ServiceResult<ListOfPlayers>()
                {
                    Data = null,
                    Message = "No registered players found for this season.",
                    Status = ResultStatus.processError
                };
            }

            return new ServiceResult<ListOfPlayers>()
            {
                Data = new ListOfPlayers { playersList = players },  // Wrap inside ListOfPlayers
                Message = "Successfully retrieved the registered player data.",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<int> Approve(int playerId)
        {
            var player = _context.RegisteredPlayers.FirstOrDefault(x => x.Id == playerId);
            if(player == null)
            {
                return new ServiceResult<int>
                {
                    Data = 0,
                    Message = "Player not found",
                    Status = ResultStatus.processError
                };
            }
            player.IsApproved = true;
            _context.SaveChanges();
            return new ServiceResult<int>
            {
                Data = 1,
                Message = "Approved successfully",
                Status = ResultStatus.Ok
            };
        }
        public int? GetUserIdFromToken(string token)
        {
            //var token = httpContext.Request.Cookies["accessToken"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return null;

            return int.TryParse(userIdClaim.Value, out int userId) ? userId : (int?)null;
        }
    }
}
