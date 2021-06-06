using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class FeedbackRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public FeedbackRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<Feedback> Add(Feedback feedback)
        {
            await programmingCourseDbContext.Feedbacks.AddAsync(feedback);
            await programmingCourseDbContext.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback> Delete(int id)
        {
            var deletedFeedback = await programmingCourseDbContext.Feedbacks.FindAsync(id);

            if (deletedFeedback == null)
            {
                return deletedFeedback;
            }

            programmingCourseDbContext.Feedbacks.Remove(deletedFeedback);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedFeedback;
        }

        public async Task<Feedback> Get(int id)
        {
            var feedback = await programmingCourseDbContext.Feedbacks
                .Where<Feedback>(f => f.Id == id)
                .Include(f => f.User).Include(f => f.Course)
                .FirstOrDefaultAsync();
            return feedback;
        }

        public async Task<IList<Feedback>> GetAll()
        {
            var feedbacks = await programmingCourseDbContext.Feedbacks
                .ToListAsync<Feedback>();
            return feedbacks;
        }

        public async Task<Feedback> Update(Feedback feedback)
        {
            if (feedback != null)
            {
                programmingCourseDbContext.Feedbacks.Update(feedback);
                await programmingCourseDbContext.SaveChangesAsync();
            }

            return feedback;
        }


        public async Task<bool> IsExistedFeedbackByStudentIdAndCourseId(string userId, int courseId)
        {
            var course = await programmingCourseDbContext.Feedbacks.Where<Feedback>(f => f.UserId == userId && f.CourseId == courseId).FirstOrDefaultAsync();

            if(course != null)
            {
                return true;
            }

            return false;
        }


        public async Task<IList<dynamic>> GetAllByCourseId(int courseId)
        {
            var feedbacks = await programmingCourseDbContext.Feedbacks
                .Where<Feedback>(f => f.CourseId == courseId)
                .Include(f => f.User)
                .Select(f => new
                {
                    Id = f.Id,
                    Rate = f.Rate,
                    Review = f.Review,
                    UserId = f.User.Id,
                    UserName = f.User.UserName,
                    UserAvatarUrl = f.User.AvatarUrl
                })
                .ToListAsync<dynamic>();
            return feedbacks;
        }
    }
}
