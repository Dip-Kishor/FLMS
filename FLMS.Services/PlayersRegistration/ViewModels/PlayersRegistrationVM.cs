using FLMS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.PlayersRegistration.ViewModels
{
    public class PlayersRegistrationVM
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public string EFootballId { get; set; }
        public string InGameName { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
    public enum GenderType
    {
        Male,
        Female,
        Others
    }
}
