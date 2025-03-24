using FLMS.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Models.FixturesAndResults
{
    public class EFixturesAndResults
    {
        [Key]
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public DateTime? MatchDate {  get; set; }
        public DateTime? MatchTime { get; set; }
        public int UserId1 { get; set; }
        public int? User1Score { get; set; }
        public int? User2Score { get; set; }
        public int UserId2 { get; set; }
        public int? TiebrekerScoreUser1 {  get; set; }
        public int? TiebrekerScoreUser2 {  get; set; }
        public GroupType Group { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCompleted { get; set; }
        public virtual ESeason Season { get; set; }
    }
    public enum GroupType
    {
        GroupA = 1,
        GroupB = 2, GroupC = 3, GroupD = 4
    }
}
