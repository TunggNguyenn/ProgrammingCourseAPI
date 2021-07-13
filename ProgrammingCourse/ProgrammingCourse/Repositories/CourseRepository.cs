using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CourseRepository : GenericRepository<Course>
    {
        public CourseRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<List<int>> GetCourseIdList()
        {
            return await _context.Set<Course>()
                .Select(c => c.Id).ToListAsync<int>();
        }


        public async Task<Course> GetWithAllInfoById(int id)
        {
            return await _context.Set<Course>()
                .Where(c => c.Id == id)
                .Include(c => c.Lectures).Include(c => c.Status).Include(c => c.Category)
                .Include(c => c.Lecturer).Include(c => c.Feedbacks).ThenInclude(c => c.User)
                .FirstOrDefaultAsync();
        }



        public async Task<List<Course>> GetByCategoryId(int categoryId)
        {
            var courses = await _context.Courses.Where<Course>(c => c.CategoryId == categoryId).ToListAsync();
            return courses;
        }

        public async Task<List<dynamic>> Get10NewestCourseIds()
        {
            var courses = await _context.Courses
                .OrderByDescending(c => c.Id)
                .Take(10)
                .Select(c => new
                {
                    CourseId = c.Id
                })
                .ToListAsync<dynamic>();
            return courses;
        }



        public async Task<dynamic> GetOutStandingCourseIds()
        {
            var courses = await _context.Set<Course>()
                .Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .OrderByDescending(c => c.StudentCourses.Count).OrderByDescending(c => c.Id)
                .Select(c => new
                {
                    CourseId = c.Id,
                })
                .Take(4)
                .ToListAsync<dynamic>();
            return courses;
        }



        public async Task<dynamic> GetBestSellerCoursesByCategoryId(int courseId, int categoryId)
        {
            var courses = await _context.Set<Course>()
                .Where<Course>(c => c.Id != courseId && c.CategoryId == categoryId)
                .Include(c => c.StudentCourses)
                .OrderByDescending(c => c.StudentCourses.Count)
                .Select(c => new
                {
                    CourseId = c.Id
                })
                .Take(5)
                .ToListAsync<dynamic>();
            return courses;
        }


        public async Task<IList<dynamic>> GetCourseListByLecturerId(string lecturerId)
        {
            var courses = await _context.Set<Course>()
                .Where<Course>(c => c.LecturerId == lecturerId)
                .Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    CategoryTypeId = c.Category.CategoryType.Id,
                    CategoryTypeName = c.Category.CategoryType.Name,
                    CategoryId = c.Category.Id,
                    CategoryName = c.Category.Name,
                    LectureId = c.Lecturer.Id,
                    LectureName = c.Lecturer.UserName,
                    ImageUrl = c.ImageUrl,
                    Price = c.Price,
                    Discount = c.Discount,
                    ShortDiscription = c.ShortDiscription,
                    DetailDiscription = c.DetailDiscription,
                    LastUpdated = c.LastUpdated,
                    StatusId = c.Status.Id,
                    StatusName = c.Status.Name,
                    RegisteredUserNumber = c.StudentCourses.Count,
                    Feedbacks = c.Feedbacks
                })
                .ToListAsync<dynamic>();
            return courses;
        }


        public async Task<Course> FindCourse(string keywords)
        {
            var course = await _context.Set<Course>()
                .Where<Course>(c => c.Name.Contains(keywords))
                .Include(c => c.Lectures).Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .FirstOrDefaultAsync();
            return course;
        }

        //public async Task<IList<Course>> GetByCategoryTypeId(int categoryTypeId)
        //{
        //    var courses = await _context.Courses
        //        .Include(c => c.Category)
        //        .Where<Course>(c => c.Category.CategoryTypeId == categoryTypeId)
        //        .ToListAsync();
        //    return courses;
        //}
    }
}
