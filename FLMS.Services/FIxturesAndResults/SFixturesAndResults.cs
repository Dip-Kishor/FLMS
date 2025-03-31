using CommonServices;
using FLMS.Data;
using FLMS.Data.SP_Result;
using FLMS.Models.FixturesAndResults;
using FLMS.Services.FIxturesAndResults.ViewModels;
using FLMS.Services.PlayersRegistration.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.FIxturesAndResults
{
    public class SFixturesAndResults
    {
        private readonly FLMSContext _context;
        public SFixturesAndResults(FLMSContext context)
        {
            _context = context;
        }
        public ServiceResult<ListOfPlayers> GetAllPlayers(int seasonId)
        {
            var players = _context.RegisteredPlayers.Where(x => x.IsApproved == true && x.SeasonId == seasonId)
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
                    userId = p.UserId
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
        public ServiceResult<FixtureAndResultCreationVM> CreateFixture(FixtureAndResultCreationVM model)
        {
            if (model == null || model.NoOfMatches <= 0 || model.NoOfGroups <= 0)
            {
                return new ServiceResult<FixtureAndResultCreationVM>
                {
                    Status = ResultStatus.processError,
                    Message = "Invalid input data."
                };
            }

            var players = model.groupPlayers.Select(p => p.Id).ToList();
            var groupedPlayers = AssignPlayersToGroups(players, model.NoOfGroups);

            var fixtures = new List<EFixturesAndResults>();

            foreach (var group in groupedPlayers)
            {
                GenerateFixtures(group.Value, model.SeasonId, model.NoOfMatches, group.Key, fixtures);
            }

            _context.FixturesAndResults.AddRange(fixtures);
            _context.SaveChanges();

            return new ServiceResult<FixtureAndResultCreationVM>
            {
                Status = ResultStatus.Ok,
                Message = "Fixtures created successfully.",
                Data = model
            };
        }
        public ServiceResult<List<FixturesAndResultsVM>> GetFixtures(int seasonId)
        {
            var result = _context.FixturesAndResults.Where(x => x.SeasonId == seasonId && x.IsPlayoff==false).ToList();
            if(result.Count==0)
            {
                return new ServiceResult<List<FixturesAndResultsVM>>()
                {
                    Data = null,
                    Message = "No fixtures for the season",
                    Status = ResultStatus.processError,
                };
            }
            var users = _context.RegisteredPlayers.Where(x => x.SeasonId == seasonId).ToList();
            var data = (from r in result
                        join u1 in users on r.UserId1 equals u1.Id into user1Data
                        from ud1 in user1Data.DefaultIfEmpty() 
                        join u2 in users on r.UserId2 equals u2.Id into user2Data
                        from ud2 in user2Data.DefaultIfEmpty() 
                        select new FixturesAndResultsVM
                        {
                            Id = r.Id,
                            SeasonId = r.SeasonId,
                            MatchDate = r.MatchDate?.ToString("yyyy-MM-dd") ?? "N/A",
                            MatchTime = r.MatchTime?.ToString() ?? "N/A",
                            UserId1 = r.UserId1,
                            ImageUrl1 = ud1?.ImageUrl,
                            UserName1 = ud1?.Name ?? "Unknown User",
                            User1Score = r.User1Score ?? 0,
                            User2Score = r.User2Score ?? 0,
                            UserId2 = r.UserId2,
                            ImageUrl2 = ud2?.ImageUrl,
                            UserName2 = ud2?.Name ?? "Unknown User", 
                            TiebrekerScoreUser1 = r.TiebrekerScoreUser1 ?? 0,
                            TiebrekerScoreUser2 = r.TiebrekerScoreUser2 ?? 0,
                            Group = r.Group,
                            IsPostponed = r.IsPostponed,
                            IsCompleted = r.IsCompleted,
                            IsPlayOff= r.IsPlayoff,
                            PlayOffType = r.PlayoffType
                        }).ToList();

            return new ServiceResult<List<FixturesAndResultsVM>>
            {
                Status = ResultStatus.Ok,
                Message ="Success",
                Data = data,
            };
        }

        public ServiceResult<FixturesAndResultsVM> UpdateFixture(FixturesAndResultsVM vm)
        {
            var data = new EFixturesAndResults
            {
                Id = vm.Id,
                SeasonId = vm.SeasonId,
                MatchDate =DateTime.Parse(vm.MatchDate),
                MatchTime = TimeSpan.Parse(vm.MatchTime),
                UserId1 = vm.UserId1,
                User1Score = vm.User1Score ?? 0,
                User2Score = vm.User2Score ?? 0,
                UserId2 = vm.UserId2,
                TiebrekerScoreUser1 = vm.TiebrekerScoreUser1 ?? 0,
                TiebrekerScoreUser2 = vm.TiebrekerScoreUser2 ?? 0,
                Group = vm.Group,
                IsPostponed = vm.IsPostponed,
                IsCompleted = vm.IsCompleted
            };
            var res = _context.FixturesAndResults.Update(data);
            _context.SaveChanges();
            return new ServiceResult<FixturesAndResultsVM>()
            {
                Data = vm,
                Message = "Successfully updated fixtures data",
                Status = ResultStatus.Ok,
            };
        }

        public async Task<ServiceResult<List<Table_SP_Results>>> GetTableAsync(int seasonId)
        {
            var tableResults = await _context.GetTableReportAsync(seasonId); 

            return new ServiceResult<List<Table_SP_Results>>
            {
                Data = tableResults,
                Message = "Success",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<FixturesAndResultsVM>> GetPlaypffData(int seasonId)
        {
            var details = _context.FixturesAndResults.Where(x=>x.SeasonId==seasonId && x.IsPlayoff==true).ToList();
            if (details.Count < 0)
            {
                return new ServiceResult<List<FixturesAndResultsVM>>()
                {
                    Data = null,
                    Message = "No playoff data available",
                    Status = ResultStatus.processError
                };
            }
            var users = _context.RegisteredPlayers.Where(x=>x.SeasonId==seasonId).ToList();
            var data = (from r in details
                        join u1 in users on r.UserId1 equals u1.Id into user1data
                        from ud1 in user1data.DefaultIfEmpty()
                        join u2 in users on r.UserId2 equals u2.Id into user2data
                        from ud2 in user2data.DefaultIfEmpty()
                        select new FixturesAndResultsVM
                        {
                            Id = r.Id,
                            SeasonId = r.SeasonId,
                            MatchDate = r.MatchDate?.ToString("yyyy-MM-dd") ?? "N/A",
                            MatchTime = r.MatchTime?.ToString() ?? "N/A",
                            UserId1 = r.UserId1,
                            ImageUrl1 = ud1?.ImageUrl,
                            UserName1 = ud1?.Name ?? "Unknown User",
                            User1Score = r.User1Score ?? 0,
                            User2Score = r.User2Score ?? 0,
                            UserId2 = r.UserId2,
                            ImageUrl2 = ud2?.ImageUrl,
                            UserName2 = ud2?.Name ?? "Unknown User",
                            TiebrekerScoreUser1 = r.TiebrekerScoreUser1 ?? 0,
                            TiebrekerScoreUser2 = r.TiebrekerScoreUser2 ?? 0,
                            Group = r.Group,
                            IsPostponed = r.IsPostponed,
                            IsCompleted = r.IsCompleted,
                            IsPlayOff = r.IsPlayoff,
                            PlayOffType = r.PlayoffType
                        }).ToList();
            return new ServiceResult<List<FixturesAndResultsVM>>()
            {
                Data = data,
                Message = "Successfully retrieved",
                Status = ResultStatus.Ok
            };
        }
        
        private Dictionary<GroupType, List<int>> AssignPlayersToGroups(List<int> players, int numberOfGroups)
        {
            var shuffledPlayers = players.OrderBy(x => Guid.NewGuid()).ToList();
            var groupedPlayers = new Dictionary<GroupType, List<int>>();

            for (int i = 0; i < numberOfGroups; i++)
            {
                groupedPlayers[(GroupType)(i + 1)] = new List<int>();
            }

            for (int i = 0; i < shuffledPlayers.Count; i++)
            {
                var group = (GroupType)((i % numberOfGroups) + 1);
                groupedPlayers[group].Add(shuffledPlayers[i]);
            }

            return groupedPlayers;
        }

        private void GenerateFixtures(List<int> players, int seasonId, int noOfMatches, GroupType group, List<EFixturesAndResults> fixtures)
        {
            for (int matchCount = 0; matchCount < noOfMatches; matchCount++)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = i + 1; j < players.Count; j++)
                    {
                        // Alternate the positions of the players for each match
                        if (matchCount % 2 == 0)
                        {
                            fixtures.Add(new EFixturesAndResults
                            {
                                SeasonId = seasonId,
                                UserId1 = players[i],
                                UserId2 = players[j],
                                Group = group,
                                IsPostponed = false,
                                IsCompleted = false,
                                MatchDate = null,
                                MatchTime = null
                            });
                        }
                        else
                        {
                            fixtures.Add(new EFixturesAndResults
                            {
                                SeasonId = seasonId,
                                UserId1 = players[j],
                                UserId2 = players[i],
                                Group = group,
                                IsPostponed = false,
                                IsCompleted = false,
                                MatchDate = null,
                                MatchTime = null
                            });
                        }
                    }
                }
            }
        }

    }
}
