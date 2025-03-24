using FLMS.Models;
using FLMS.Models.FixturesAndResults;
using FLMS.Models.PlayerRegistration;
using FLMS.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Data
{
    public class FLMSContext : DbContext
    {
        public FLMSContext(DbContextOptions<FLMSContext> options)
       : base(options)
        {
        }
        public DbSet<EUser> Users { get; set; }
        public DbSet<EUserRole> UserRoles { get; set; }
        public DbSet<ERegisteredPlayers> RegisteredPlayers { get; set; }
        public DbSet<ESeason> Seasons { get; set; }
        public DbSet<EFixturesAndResults> FixturesAndResults { get; set; }
    }
}
