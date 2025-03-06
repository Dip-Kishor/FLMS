using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Models.User
{
    public class EUserRole
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public virtual ICollection<EUser> Users { get; set; }
    }
}
