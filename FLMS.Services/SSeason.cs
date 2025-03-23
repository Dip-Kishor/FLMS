using CommonServices;
using FLMS.Services.PlayersRegistration.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLMS.Web.Areas.ViewModels;
using FLMS.Data;

namespace FLMS.Services
{
    public class SSeason
    {
        private readonly FLMSContext _context;
        public SSeason(FLMSContext context)
        {
            _context = context;
        }
        public ServiceResult<SeasonVM> GetAllSeasons()
        {
            var seasons = _context.Seasons
                .Select(p => new SeasonVMs
                {
                    id = p.Id,
                    seasonName = p.SeasonName,
                    startDate = p.StartDate.ToString("yyyy-MM-dd"),
                    endDate = p.EndDate.ToString("yyyy-MM-dd"),
                    isCurrentSeason = p.IsCurrentSeason,
                })
                .ToList();

            if (seasons == null)  // Handle empty results properly
            {
                return new ServiceResult<SeasonVM>()
                {
                    Data = null,
                    Message = "No seasons avalable.",
                    Status = ResultStatus.processError
                };
            }

            return new ServiceResult<SeasonVM>()
            {
                Data = new SeasonVM { seasonVMs = seasons },  // Wrap inside ListOfPlayers
                Message = "Successfully retrieved the registered player data.",
                Status = ResultStatus.Ok
            };
        }
    }
}
