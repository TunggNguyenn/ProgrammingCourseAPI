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
            var course = await programmingCourseDbContext.Courses.Where<Course>(c => c.Id == id).Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.Category).FirstOrDefaultAsync();
            return course;
        }

        public async Task<IList<Course>> GetAll()
        {
            var courses = await programmingCourseDbContext.Courses.Include(c => c.Lecturer).Include(c => c.Status).Include(c => c.Category).ToListAsync<Course>();
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
    }
}
