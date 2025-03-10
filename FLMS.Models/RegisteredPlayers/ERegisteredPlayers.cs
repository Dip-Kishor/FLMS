using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Models.RegisteredPlayers
{
    public class ERegisteredPlayers
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public string EFootballId { get; set; }
        public string InGameName { get; set; }
        public string ImageUrl { get; set; }
        public virtual ESeason Season { get; set; }
    }
    public enum GenderType
    {
        Male,
        Female,
        Others
    }
}
