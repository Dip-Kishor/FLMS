using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Models
{
    public class ESeason
    {
        [Key]
        public int Id { get; set; }
        public string SeasonName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentSeason { get; set; }
    }
}
