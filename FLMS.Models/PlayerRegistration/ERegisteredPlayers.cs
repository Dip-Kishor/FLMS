using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Models.PlayerRegistration
{
    public class ERegisteredPlayers
    {
        [Key]
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public string EFootballId { get; set; }
        public string InGameName { get; set; }
        public string? ImageUrl { get; set; }
        public string? TeamImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public virtual ESeason Season { get; set; }
    }
    public enum GenderType
    {
        Male=0,
        Female =1,
        Others=2
    }
}
