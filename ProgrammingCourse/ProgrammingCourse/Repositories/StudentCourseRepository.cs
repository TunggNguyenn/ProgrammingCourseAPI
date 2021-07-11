using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class StudentCourseRepository : GenericRepository<StudentCourse>
    {
        public StudentCourseRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }


        public async Task<List<dynamic>> GetStudentCourseListByStudentId(string studentId)
        {
            var studentCourses = await _context.StudentCourses
                .Where<StudentCourse>(sc => sc.StudentId == studentId)
                .Include(sc => sc.Student).Include(sc => sc.Course)
                .Select(sc => new
                {
                    Id = sc.Id,
                    StudentId = sc.Student.Id,
                    StudentUserName = sc.Student.UserName,
                    CourseId = sc.Course.Id,
                    CourseName = sc.Course.Name,
                    ImageUrl = sc.Course.ImageUrl,
                    Price = sc.Course.Price,
                    Discount = sc.Course.Discount,
                    ShortDiscription = sc.Course.ShortDiscription,
                    DetailDiscription = sc.Course.DetailDiscription,
                    LastUpdated = sc.Course.LastUpdated
                })
                .ToListAsync<dynamic>();
            return studentCourses;
        }


        public async Task<List<dynamic>> Get10BestSellerCourseIDsInMonth()
        {
            var studentCourses = await _context.Set<StudentCourse>()
                .Where<StudentCourse>(sc => sc.DateTime.Month == DateTime.Now.Month && sc.DateTime.Year == DateTime.Now.Year)
                .GroupBy(sc => sc.CourseId)
                .Select(sc => new
                {
                    CourseId = sc.Key,
                    Count = sc.Count()
                })
                .OrderByDescending(c => c.Count)
                .Take(10)
                .ToListAsync<dynamic>();
            return studentCourses;
        }

        public async Task<List<dynamic>> Get10BestSellerCoursesInMonthByCategoryTypeId(int categoryTypeId)
        {
            var studentCourses = await _context.Set<StudentCourse>()
                .Include(sc => sc.Course).ThenInclude(sc => sc.Category)
                .Where<StudentCourse>(sc => sc.Course.Category.CategoryTypeId == categoryTypeId && sc.DateTime.Month == DateTime.Now.Month && sc.DateTime.Year == DateTime.Now.Year)
                .GroupBy(sc => sc.CourseId)
                .Select(sc => new
                {
                    CourseId = sc.Key,
                    Count = sc.Count()
                })
                .OrderByDescending(c => c.Count)
                .Take(10)
                .ToListAsync<dynamic>();

            return studentCourses;
        }


        public async Task<int> GetRegisteredNumberByCourseId(int courseId)
        {
            return await _context.StudentCourses.Where<StudentCourse>(sc => sc.CourseId == courseId).CountAsync();
        }


        public async Task<IList<dynamic>> GetStudentCourseListByCourseId(int courseId)
        {
            var studentCourses = await _context.Set<StudentCourse>()
                .Where<StudentCourse>(sc => sc.CourseId == courseId)
                .Include(sc => sc.Student).Include(sc => sc.Course)
                .Select(sc => new
                {
                    Id = sc.Id,
                    StudentId = sc.Student.Id,
                    StudentUserName = sc.Student.UserName,
                    StudentAvatarUrl = sc.Student.AvatarUrl,
                    StudentEmail = sc.Student.Email,
                    CourseId = sc.Course.Id,
                    CourseName = sc.Course.Name,
                })
                .ToListAsync<dynamic>();
            return studentCourses;
        }


        public async Task<bool> IsParticipatedByStudentIdAndCourseId(string studentId, int courseId)
        {
            var studentCourse = await _context.StudentCourses
                            .Where<StudentCourse>(sc => sc.StudentId == studentId && sc.CourseId == courseId)
                            .FirstOrDefaultAsync();

            if (studentCourse != null)
            {
                return true;
            }

            return false;
        }


        //public async Task<List<StudentCourse>> GetRegisteredStudentCoursesInMonthByCourseId(int courseId)
        //{
        //    var studentCourses = await _context.Set<StudentCourse>()
        //        .Where<StudentCourse>(sc => sc.CourseId == courseId && sc.DateTime.Month == DateTime.Now.Month && sc.DateTime.Year == DateTime.Now.Year)
        //        .ToListAsync<StudentCourse>();
        //    return studentCourses;
        //}

    }
}
