using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class LectureRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public LectureRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<Lecture> Add(Lecture lecture)
        {
            await programmingCourseDbContext.Lectures.AddAsync(lecture);
            await programmingCourseDbContext.SaveChangesAsync();
            return lecture;
        }

        public async Task<Lecture> Delete(int id)
        {
            var deletedLecture = await programmingCourseDbContext.Lectures.FindAsync(id);

            if (deletedLecture == null)
            {
                return deletedLecture;
            }

            programmingCourseDbContext.Lectures.Remove(deletedLecture);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedLecture;
        }

        public async Task<Lecture> Get(int id)
        {
            var lecture = await programmingCourseDbContext.Lectures.Where<Lecture>(l => l.Id == id).Include(l => l.Course).FirstOrDefaultAsync();
            return lecture;
        }

        public async Task<IList<Lecture>> GetAll()
        {
            var lectures = await programmingCourseDbContext.Lectures.ToListAsync<Lecture>();
            return lectures;
        }

        public async Task<Lecture> Update(Lecture lecture)
        {
            if (lecture != null)
            {
                programmingCourseDbContext.Lectures.Update(lecture);
                await programmingCourseDbContext.SaveChangesAsync();
            }

            return lecture;
        }


        public async Task<IList<Lecture>> GetAllByCourseId(int courseId)
        {
            var lectures = await programmingCourseDbContext.Lectures
                .Where<Lecture>(l => l.CourseId == courseId)
                .ToListAsync<Lecture>();
            return lectures;
        }
    }
}
