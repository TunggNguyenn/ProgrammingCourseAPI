using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class LectureRepository : GenericRepository<Lecture>
    {
        public LectureRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }



        public async Task<IList<Lecture>> GetLectureListByCourseId(int courseId)
        {
            var lectures = await _context.Set<Lecture>()
                .Where<Lecture>(l => l.CourseId == courseId)
                .ToListAsync<Lecture>();
            return lectures;
        }

        public async Task<double> GetCompletionRateByCourseIdAndLectureId(int courseId, int lectureId)
        {
            var lectureNumber = await _context.Set<Lecture>().Where(l => l.CourseId == courseId).CountAsync();

            var lecture = await _context.Set<Lecture>().Where(l => l.Id == lectureId).FirstOrDefaultAsync();

            double completionRate = (double)lecture.Section / lectureNumber;
            return completionRate;
        }
    }
}
