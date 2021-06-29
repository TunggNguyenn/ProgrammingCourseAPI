using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class WatchListRepository : GenericRepository<WatchList>
    {
        public WatchListRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }


        //public async Task<IList<dynamic>> GetAllByStudentId(string studentId)
        //{
        //    var watchLists = await programmingCourseDbContext.WatchLists
        //        .Where<WatchList>(w => w.StudentId == studentId)
        //        .Include(w => w.Student).Include(w => w.Course).ThenInclude(c => c.Category).ThenInclude(c => c.CategoryType)
        //        .Select(w => new
        //        {
        //            Id = w.Id,
        //            StudentId = w.Student.Id,
        //            StudentUserName = w.Student.UserName,
        //            StudentEmail = w.Student.Email,
        //            CourseId = w.Course.Id,
        //            CourseName = w.Course.Name,
        //            CategoryTypeId = w.Course.Category.CategoryType.Id,
        //            CategoryTypeName = w.Course.Category.CategoryType.Name,
        //            CategoryId = w.Course.Category.Id,
        //            CategoryName = w.Course.Category.Name,
        //            LectureId = w.Course.Lecturer.Id,
        //            LectureName = w.Course.Lecturer.UserName,
        //            ImageUrl = w.Course.ImageUrl,
        //            Price = w.Course.Price,
        //            Discount = w.Course.Discount,
        //            ShortDiscription = w.Course.ShortDiscription,
        //            DetailDiscription = w.Course.DetailDiscription,
        //            LastUpdated = w.Course.LastUpdated,
        //            StatusId = w.Course.Status.Id,
        //            StatusName = w.Course.Status.Name,
        //            RegisteredUserNumber = w.Course.StudentCourses.Count,
        //            //Feedbacks = w.Course.Feedbacks
        //        })
        //        .ToListAsync<dynamic>();
        //    return watchLists;
        //}


        public async Task<bool> IsExistedWatchListByStudentIdAndCourseId(string userId, int courseId)
        {
            var watchList = await _context.WatchLists
                .Where<WatchList>(w => w.StudentId == userId && w.CourseId == courseId)
                .FirstOrDefaultAsync();

            if (watchList != null)
            {
                return true;
            }

            return false;
        }
    }
}
