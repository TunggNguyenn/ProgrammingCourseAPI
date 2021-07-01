using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class ViewRepository : GenericRepository<View>
    {
        public ViewRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<View> GetByCouseIdAndDateTime(int courseId, DateTime dateTime)
        {
            return await _context.Set<View>()
                .Where<View>(v => v.CourseId == courseId && v.DateTime.Day == dateTime.Day && v.DateTime.Month == dateTime.Month && v.DateTime.Year == dateTime.Year)
                .FirstOrDefaultAsync();

        }
    }
}
