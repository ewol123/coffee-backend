using auth_server;
using auth_server.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace auth_server.Repositories
{
    public class AuthRepository: IDisposable
    {
        ApplicationDbContext db = ApplicationDbContext.Create();
        

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = db.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            db.RefreshTokens.Add(token);

            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await db.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                db.RefreshTokens.Remove(refreshToken);
                return await db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            db.RefreshTokens.Remove(refreshToken);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await db.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return db.RefreshTokens.ToList();
        }

        public void Dispose()
        {
            db.Dispose();

        }
    }
}