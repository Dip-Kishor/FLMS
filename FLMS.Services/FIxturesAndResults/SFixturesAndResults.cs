using CommonServices;
using FLMS.Data;
using FLMS.Models.FixturesAndResults;
using FLMS.Services.FIxturesAndResults.ViewModels;
using FLMS.Services.PlayersRegistration.ViewModels;
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
                }
            }
        }
    }
}
