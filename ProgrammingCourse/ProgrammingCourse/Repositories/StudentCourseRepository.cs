using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class StudentCourseRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public StudentCourseRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<StudentCourse> Add(StudentCourse studentCourse)
        {
            await programmingCourseDbContext.StudentCourses.AddAsync(studentCourse);
            await programmingCourseDbContext.SaveChangesAsync();
            return studentCourse;
        }

        public async Task<StudentCourse> Delete(int id)
        {
            var deletedStudentCourse = await programmingCourseDbContext.StudentCourses.FindAsync(id);

            if (deletedStudentCourse == null)
            {
                return deletedStudentCourse;
            }

            programmingCourseDbContext.StudentCourses.Remove(deletedStudentCourse);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedStudentCourse;
        }

        public async Task<StudentCourse> Get(int id)
        {
            var studentCourse = await programmingCourseDbContext.StudentCourses
                .Where<StudentCourse>(sc => sc.Id == id)
                .Include(sc => sc.Student).Include(sc => sc.Course)
                .FirstOrDefaultAsync();
            return studentCourse;
        }

        public async Task<IList<dynamic>> GetAll()
        {
            var studentCourses = await programmingCourseDbContext.StudentCourses
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
                    View = sc.Course.View,
                    ShortDiscription = sc.Course.ShortDiscription,
                    DetailDiscription = sc.Course.DetailDiscription,
                    LastUpdated = sc.Course.LastUpdated
                })
                .ToListAsync<dynamic>();

            return studentCourses;
        }

        public async Task<StudentCourse> Update(StudentCourse studentCourse)
        {
            if (studentCourse != null)
            {
                programmingCourseDbContext.StudentCourses.Update(studentCourse);
                await programmingCourseDbContext.SaveChangesAsync();
            }

            return studentCourse;
        }



        public async Task<IList<dynamic>> GetAllByStudentId(string studentId)
        {
            var studentCourses = await programmingCourseDbContext.StudentCourses
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
                    View = sc.Course.View,
                    ShortDiscription = sc.Course.ShortDiscription,
                    DetailDiscription = sc.Course.DetailDiscription,
                    LastUpdated = sc.Course.LastUpdated
                })
                .ToListAsync<dynamic>();
            return studentCourses;
        }


        public async Task<IList<dynamic>> GetAllByCourseId(int courseId)
        {
            var studentCourses = await programmingCourseDbContext.StudentCourses
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
            var studentCourses = await programmingCourseDbContext.StudentCourses
                            .Where<StudentCourse>(sc => sc.StudentId == studentId && sc.CourseId == courseId)
                            .FirstOrDefaultAsync();

            if(studentCourses != null)
            {
                return true;
            }

            return false;
        }
    }
}
