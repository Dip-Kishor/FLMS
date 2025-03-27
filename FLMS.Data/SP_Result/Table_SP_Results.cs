using FLMS.Models.FixturesAndResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Data.SP_Result
{
    public class Table_SP_Results
    {
        public int Group { get; set; }
        public long Rank { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string ImageUrl { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GF {  get; set; }
        public int GD {  get; set; }
        public int GA {  get; set; }
        public int Points { get; set; }
    }

}
