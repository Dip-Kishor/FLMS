using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.TokenValidation
{
    public interface ITokenBlacklistService
    {
        Task<bool> IsTokenRevoked(string token);
        Task RevokeToken(string token);
    }
}
