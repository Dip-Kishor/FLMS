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
    }
}
