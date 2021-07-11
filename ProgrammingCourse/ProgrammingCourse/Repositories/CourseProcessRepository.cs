using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CourseProcessRepository  : GenericRepository<CourseProcess>
    {
        public CourseProcessRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<CourseProcess> GetByStudentIdAndCourseId(string studentId, int courseId)
        {
            return await _context.Set<CourseProcess>().Where(cp => cp.StudentId == studentId && cp.CourseId == courseId).FirstOrDefaultAsync();
        }
    }
}
