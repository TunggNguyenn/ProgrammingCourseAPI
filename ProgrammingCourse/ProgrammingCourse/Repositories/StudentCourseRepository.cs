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
            var studentCourse = await programmingCourseDbContext.StudentCourses.Where<StudentCourse>(sc => sc.Id == id).Include(sc => sc.Student).Include(sc => sc.Course).FirstOrDefaultAsync();
            return studentCourse;
        }

        public async Task<IList<StudentCourse>> GetAll()
        {
            var studentCourses = await programmingCourseDbContext.StudentCourses.Include(sc => sc.Student).Include(sc => sc.Course).ToListAsync<StudentCourse>();
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
    }
}
