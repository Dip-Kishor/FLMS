using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.User.ViewModels
{
    public record UserSession(int UserId,string Username, string Email, string Role);
}
