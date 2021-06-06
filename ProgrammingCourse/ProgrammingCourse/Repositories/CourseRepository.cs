using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CourseRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public CourseRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<Course> Add(Course course)
        {
            await programmingCourseDbContext.Courses.AddAsync(course);
            await programmingCourseDbContext.SaveChangesAsync();
            return course;
        }

        public async Task<Course> Delete(int id)
        {
            var deletedCourse = await programmingCourseDbContext.Courses.FindAsync(id);

            if (deletedCourse == null)
            {
                return deletedCourse;
            }

            programmingCourseDbContext.Courses.Remove(deletedCourse);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedCourse;
        }

        public async Task<Course> Get(int id)
        {
            var course = await programmingCourseDbContext.Courses
                .Where<Course>(c => c.Id == id)
                .Include(c => c.Lectures).Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .FirstOrDefaultAsync();
            return course;
        }

        public async Task<IList<dynamic>> GetAll()
        {
            var courses = await programmingCourseDbContext.Courses
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
                    View = c.View,
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

        public async Task<Course> Update(Course course)
        {
            if (course != null)
            {
                programmingCourseDbContext.Courses.Update(course);
                await programmingCourseDbContext.SaveChangesAsync();
            }

            return course;
        }


        public async Task<IList<Course>> GetByCategoryId(int categoryId)
        {
            var courses = await programmingCourseDbContext.Courses.Where<Course>(c => c.CategoryId == categoryId).ToListAsync();
            return courses;
        }


        public async Task<dynamic> GetOutStandingCourses()
        {
            var courses = await programmingCourseDbContext.Courses
                .Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .OrderByDescending(c => c.View).ThenByDescending(c => c.StudentCourses.Count)
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
                    View = c.View,
                    ShortDiscription = c.ShortDiscription,
                    DetailDiscription = c.DetailDiscription,
                    LastUpdated = c.LastUpdated,
                    StatusId = c.Status.Id,
                    StatusName = c.Status.Name,
                    RegisteredUserNumber = c.StudentCourses.Count,
                    Feedbacks = c.Feedbacks
                })
                .Take(4)
                .ToListAsync<dynamic>();
            return courses;
        }

        public async Task<dynamic> GetMostViewedCourses()
        {
            var courses = await programmingCourseDbContext.Courses
                .Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .OrderByDescending(c => c.View)
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
                    View = c.View,
                    ShortDiscription = c.ShortDiscription,
                    DetailDiscription = c.DetailDiscription,
                    LastUpdated = c.LastUpdated,
                    StatusId = c.Status.Id,
                    StatusName = c.Status.Name,
                    RegisteredUserNumber = c.StudentCourses.Count,
                    Feedbacks = c.Feedbacks
                })
                .Take(10)
                .ToListAsync<dynamic>();
            return courses;
        }



        public async Task<dynamic> GetNewestCourses()
        {
            var courses = await programmingCourseDbContext.Courses
                .Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .OrderByDescending(c => c.Id)
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
                    View = c.View,
                    ShortDiscription = c.ShortDiscription,
                    DetailDiscription = c.DetailDiscription,
                    LastUpdated = c.LastUpdated,
                    StatusId = c.Status.Id,
                    StatusName = c.Status.Name,
                    RegisteredUserNumber = c.StudentCourses.Count,
                    Feedbacks = c.Feedbacks
                })
                .Take(10)
                .ToListAsync<dynamic>();
            return courses;
        }


        public async Task<dynamic> GetBestSellerCoursesByCategoryId(int courseId, int categoryId)
        {
            var courses = await programmingCourseDbContext.Courses
                .Where<Course>(c => c.Id != courseId && c.CategoryId == categoryId)
                .Include(c => c.Feedbacks).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.StudentCourses).Include(c => c.Category)
                .ThenInclude(c => c.CategoryType)
                .OrderByDescending(c => c.Id)
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
                    View = c.View,
                    ShortDiscription = c.ShortDiscription,
                    DetailDiscription = c.DetailDiscription,
                    LastUpdated = c.LastUpdated,
                    StatusId = c.Status.Id,
                    StatusName = c.Status.Name,
                    RegisteredUserNumber = c.StudentCourses.Count,
                    Feedbacks = c.Feedbacks
                })
                .Take(5)
                .ToListAsync<dynamic>();
            return courses;
        }


        public async Task<IList<dynamic>> GetAllByLecturerId(string lecturerId)
        {
            var courses = await programmingCourseDbContext.Courses
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
                    View = c.View,
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
    }
}
