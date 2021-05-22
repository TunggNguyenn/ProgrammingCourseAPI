using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class RefreshTokenRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public RefreshTokenRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<RefreshToken> Add(RefreshToken refreshToken)
        {
            await programmingCourseDbContext.RefreshTokens.AddAsync(refreshToken);
            await programmingCourseDbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken> Delete(int id)
        {
            var deletedRefreshToken = await programmingCourseDbContext.RefreshTokens.FindAsync(id);

            if (deletedRefreshToken == null)
            {
                return deletedRefreshToken;
            }

            deletedRefreshToken.RevokedOn = DateTime.UtcNow;

            programmingCourseDbContext.Update(deletedRefreshToken);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedRefreshToken;
        }

        public RefreshToken Get(int id)
        {
            var refreshToken = programmingCourseDbContext.RefreshTokens.Where<RefreshToken>(r => r.Id == id).FirstOrDefault();
            return refreshToken;
        }

        public IList<RefreshToken> GetAll()
        {
            var refreshTokens = programmingCourseDbContext.RefreshTokens.ToList<RefreshToken>();
            return refreshTokens;
        }

        public IList<RefreshToken> GetByUserId(string userId)
        {
            var refreshTokens = programmingCourseDbContext.RefreshTokens.Where<RefreshToken>(r => r.UserId == userId).ToList<RefreshToken>();
            return refreshTokens;
        }

        public RefreshToken GetByUserIdAndToken(string userId, string token)
        {
            var refreshToken = programmingCourseDbContext.RefreshTokens.Where<RefreshToken>(r => r.UserId == userId && r.Token == token && r.RevokedOn == null).FirstOrDefault();
            return refreshToken;
        }

        public async Task<RefreshToken> Update(RefreshToken refreshToken)
        {
            programmingCourseDbContext.RefreshTokens.Update(refreshToken);
            await programmingCourseDbContext.SaveChangesAsync();

            return refreshToken;
        }
    }
}
