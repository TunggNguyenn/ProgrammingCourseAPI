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

        public async Task<Feedback> GetFeedbackByStudentIdAndCourseId(string userId, int courseId)
        {
            return await _context.Feedbacks.Where<Feedback>(f => f.UserId == userId && f.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task<double> GetRatingByCourseId(int courseId)
        {
            var feedbacks = await _context.Feedbacks.Where<Feedback>(f => f.CourseId == courseId).ToListAsync();

            double rating = 0.0f;
            foreach(var f in feedbacks)
            {
                rating += f.Rate;
            }

            return (double)rating / (feedbacks.Count == 0 ? 1 : feedbacks.Count);
        }

        public async Task<List<Feedback>> GetFeedbackListByCourseId(int courseId)
        {
            return await _context.Feedbacks
                .Where<Feedback>(f => f.CourseId == courseId)
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<int> GetReviewerNumberByCourseId(int courseId)
        {
            var feedbacks = await _context.Feedbacks.Where<Feedback>(f => f.CourseId == courseId).ToListAsync();
            return feedbacks.Count;
        }
    }
}
