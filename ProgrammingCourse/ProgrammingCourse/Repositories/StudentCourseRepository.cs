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


        //public async Task<IList<dynamic>> GetAllByStudentId(string studentId)
        //{
        //    var studentCourses = await programmingCourseDbContext.StudentCourses
        //        .Where<StudentCourse>(sc => sc.StudentId == studentId)
        //        .Include(sc => sc.Student).Include(sc => sc.Course)
        //        .Select(sc => new
        //        {
        //            Id = sc.Id,
        //            StudentId = sc.Student.Id,
        //            StudentUserName = sc.Student.UserName,
        //            CourseId = sc.Course.Id,
        //            CourseName = sc.Course.Name,
        //            ImageUrl = sc.Course.ImageUrl,
        //            Price = sc.Course.Price,
        //            Discount = sc.Course.Discount,
        //            ShortDiscription = sc.Course.ShortDiscription,
        //            DetailDiscription = sc.Course.DetailDiscription,
        //            LastUpdated = sc.Course.LastUpdated
        //        })
        //        .ToListAsync<dynamic>();
        //    return studentCourses;
        //}


        //public async Task<IList<dynamic>> GetAllByCourseId(int courseId)
        //{
        //    var studentCourses = await programmingCourseDbContext.StudentCourses
        //        .Where<StudentCourse>(sc => sc.CourseId == courseId)
        //        .Include(sc => sc.Student).Include(sc => sc.Course)
        //        .Select(sc => new
        //        {
        //            Id = sc.Id,
        //            StudentId = sc.Student.Id,
        //            StudentUserName = sc.Student.UserName,
        //            StudentAvatarUrl = sc.Student.AvatarUrl,
        //            StudentEmail = sc.Student.Email,
        //            CourseId = sc.Course.Id,
        //            CourseName = sc.Course.Name,
        //        })
        //        .ToListAsync<dynamic>();
        //    return studentCourses;
        //}


        public async Task<bool> IsParticipatedByStudentIdAndCourseId(string studentId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                            .Where<StudentCourse>(sc => sc.StudentId == studentId && sc.CourseId == courseId)
                            .FirstOrDefaultAsync();

            if (studentCourses != null)
            {
                return true;
            }

            return false;
        }
    }
}
