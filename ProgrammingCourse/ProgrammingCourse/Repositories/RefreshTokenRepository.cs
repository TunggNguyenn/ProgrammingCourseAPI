using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>
    {
        public RefreshTokenRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken> Remove(int id)
        {
            Console.WriteLine("Entered");
            var deletedRefreshToken = await _context.RefreshTokens.FindAsync(id);

            if (deletedRefreshToken == null)
            {
                return deletedRefreshToken;
            }

            deletedRefreshToken.RevokedOn = DateTime.UtcNow;

            _context.RefreshTokens.Update(deletedRefreshToken);
            await _context.SaveChangesAsync();

            return deletedRefreshToken;
        }


        public IList<RefreshToken> GetByUserId(string userId)
        {
            var refreshTokens = _context.RefreshTokens.Where<RefreshToken>(r => r.UserId == userId).ToList<RefreshToken>();
            return refreshTokens;
        }

        public RefreshToken GetByUserIdAndToken(string userId, string token)
        {
            var refreshToken = _context.RefreshTokens.Where<RefreshToken>(r => r.UserId == userId && r.Token == token && r.RevokedOn == DateTime.MinValue).FirstOrDefault();
            return refreshToken;
        }
    }
}
