using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Services.TokenValidation
{
    public class STokenBlacklistService : ITokenBlacklistService
    {
        private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

        public Task<bool> IsTokenRevoked(string token)
        {
            return Task.FromResult(_revokedTokens.ContainsKey(token));
        }

        public Task RevokeToken(string token)
        {
            _revokedTokens[token] = DateTime.UtcNow.AddDays(1); // Set expiry for cleanup
            return Task.CompletedTask;
        }
    }
}
