using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class WatchListRepository : GenericRepository<WatchList>
    {

        public WatchListRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }


        public async Task<List<dynamic>> GetWatchListListByStudentId(string studentId)
        {
            var watchLists = await _context.Set<WatchList>()
                .Where<WatchList>(w => w.StudentId == studentId)
                .Include(w => w.Student).Include(w => w.Course)
                .ToListAsync<WatchList>();

            List<dynamic> dynamicWatchLists = new List<dynamic>();

            for (int i = 0; i < watchLists.Count; i++)
            {
                dynamic dynamicWatchList = new ExpandoObject();

                dynamicWatchList.id = watchLists[i].Id;
                dynamicWatchList.studentId = watchLists[i].Student.Id;
                dynamicWatchList.studentUserName = watchLists[i].Student.UserName;
                dynamicWatchList.studentEmail = watchLists[i].Student.Email;
                dynamicWatchList.courseId = watchLists[i].Course.Id;
                dynamicWatchList.courseName = watchLists[i].Course.Name;
                //dynamicWatchList.CategoryTypeId = watchLists[i].Course.Category.CategoryType.Id;
                //dynamicWatchList.CategoryTypeName = watchLists[i].Course.Category.CategoryType.Name;
                //dynamicWatchList.CategoryId = watchLists[i].Course.Category.Id;
                //dynamicWatchList.CategoryName = watchLists[i].Course.Category.Name;
                //dynamicWatchList.LectureId = watchLists[i].Course.Lecturer.Id;
                //dynamicWatchList.LectureName = watchLists[i].Course.Lecturer.UserName;
                //dynamicWatchList.LecturerAvatar = watchLists[i].Course.Lecturer.AvatarUrl;
                dynamicWatchList.lecturer = await GetLecturerInfoById(watchLists[i].Course.LecturerId);
                dynamicWatchList.imageUrl = watchLists[i].Course.ImageUrl;
                dynamicWatchList.price = watchLists[i].Course.Price;
                dynamicWatchList.discount = watchLists[i].Course.Discount;
                dynamicWatchList.lastUpdated = watchLists[i].Course.LastUpdated;
                //dynamicWatchList.statusId = watchLists[i].Course.Status.Id;
                //dynamicWatchList.statusName = watchLists[i].Course.Status.Name;
                dynamicWatchList.registeredUserNumber = await GetRegisteredNumberByCourseId(watchLists[i].CourseId);
                dynamicWatchList.rating = await GetRatingByCourseId(watchLists[i].CourseId);

                dynamicWatchLists.Add(dynamicWatchList);
            }

            return dynamicWatchLists;
        }

        private async Task<object> GetLecturerInfoById(string id)
        {
            return await _context.Set<User>().Where<User>(usr => usr.Id == id)
                .Select(usr => new
                {
                    Id = usr.Id,
                    AvatarUrl = usr.AvatarUrl,
                    UserName = usr.UserName,
                    Email = usr.Email,
                    Description = usr.Description
                })
                .FirstOrDefaultAsync();
        }

        private async Task<double> GetRatingByCourseId(int courseId)
        {
            var feedbacks = await _context.Feedbacks.Where<Feedback>(f => f.CourseId == courseId).ToListAsync();

            double rating = 0.0f;
            foreach (var f in feedbacks)
            {
                rating += f.Rate;
            }

            return (double)rating / (feedbacks.Count == 0 ? 1 : feedbacks.Count);
        }


        private async Task<int> GetRegisteredNumberByCourseId(int courseId)
        {
            return await _context.StudentCourses.Where<StudentCourse>(sc => sc.CourseId == courseId).CountAsync();
        }

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
