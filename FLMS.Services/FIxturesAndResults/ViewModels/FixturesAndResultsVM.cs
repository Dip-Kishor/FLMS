using FLMS.Models;
using FLMS.Models.FixturesAndResults;
using FLMS.Services.PlayersRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.FIxturesAndResults.ViewModels
{
    public class FixturesAndResultsVM
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public DateTime MatchDate { get; set; }
        public DateTime MatchTime { get; set; }
        public int UserId1 { get; set; }
        public string UserName1 { get; set; }
        public int? User1Score { get; set; }
        public int? User2Score { get; set; }
        public int UserId2 { get; set; }
        public string UserName2 { get; set; }
        public int? TiebrekerScoreUser1 { get; set; }
        public int? TiebrekerScoreUser2 { get; set; }
        public GroupType? IsGroupA { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCompleted { get; set; }
    }
   
    public class FixtureAndResultCreationVM
    {
        public int SeasonId { get; set;}
        public int NoOfMatches { get; set; }
        public int NoOfGroups { get; set; }
        public List<GroupPlayers> groupPlayers { get; set; }
    }
    public class GroupPlayers
    {
        public int Id { get; set; }
    }
   
}
