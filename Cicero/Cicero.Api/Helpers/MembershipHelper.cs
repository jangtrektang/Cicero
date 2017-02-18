using Cicero.Core.DataObjects;
using Cicero.Core.Helpers.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace Cicero.Api.Helpers
{
    public class MembershipHelper
    {
        private CiceroContext _db = new CiceroContext();

        public async Task<User> FindUser(string username, string password)
        {
            var passwordHashed = new PasswordHelper().HashPassword(password);
            var user = await _db.Users
                 .Where(u => u.Username == username && u.Password == passwordHashed)
                 .FirstOrDefaultAsync();

            return user;
        }

        public Client FindClient(string clientID)
        {
            var client = _db.Clients.Find(clientID);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            // Check if token exists for this user & client
            var existingToken = _db.RefreshTokens
                .Where(r => r.Subject == token.Subject && r.ClientID == token.ClientID)
                .SingleOrDefault();

            // Remove existing token
            if(existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            // Add new token
            _db.RefreshTokens.Add(token);

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string TokenID)
        {
            // Find refreshtoken by ID
            var refreshToken = await _db.RefreshTokens.FindAsync(TokenID);

            if(refreshToken != null)
            {
                // Remove refreshtoken
                _db.RefreshTokens.Remove(refreshToken);
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken token)
        {
            _db.RefreshTokens.Remove(token);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string tokenID)
        {
            var token = await _db.RefreshTokens.FindAsync(tokenID);

            return token;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _db.RefreshTokens.ToList();
        }
    }
}