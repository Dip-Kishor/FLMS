using FLMS.Models;
using FLMS.Models.PlayerRegistration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.PlayersRegistration.ViewModels
{
    public class  ListOfPlayers
    {
        public List<PlayersRegistrationVM> playersList {  get; set; }
    }
    public class PlayersRegistrationVM
    {
        public int id { get; set; }
        public int seasonId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public GenderType gender { get; set; }
        public string eFootballId { get; set; }
        public string inGameName { get; set; }
        public string? imageUrl { get; set; }
        public string? teamImageUrl { get; set; }
        public bool isApproved { get; set; }
    }
    
}
