using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>
    {
        public FeedbackRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }



        public async Task<bool> IsExistedFeedbackByStudentIdAndCourseId(string userId, int courseId)
        {
            var course = await _context.Feedbacks.Where<Feedback>(f => f.UserId == userId && f.CourseId == courseId).FirstOrDefaultAsync();

            if (course != null)
            {
                return true;
            }

            return false;
        }


        //public async Task<IList<dynamic>> GetAllByCourseId(int courseId)
        //{
        //    var feedbacks = await programmingCourseDbContext.Feedbacks
        //        .Where<Feedback>(f => f.CourseId == courseId)
        //        .Include(f => f.User)
        //        .Select(f => new
        //        {
        //            Id = f.Id,
        //            Rate = f.Rate,
        //            Review = f.Review,
        //            UserId = f.User.Id,
        //            UserName = f.User.UserName,
        //            UserAvatarUrl = f.User.AvatarUrl
        //        })
        //        .ToListAsync<dynamic>();
        //    return feedbacks;
        //}
    }
}
