using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CourseCartRepository : GenericRepository<CourseCart>
    {
        public CourseCartRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<List<CourseCart>> RemoveByCourseIdsAndCartId(List<int> courseIds, int cartId)
        {
            var removedCourseCarts = await _context.Set<CourseCart>().Where(cc => courseIds.Contains(cc.CourseId) && cc.CartId == cartId).ToListAsync();

            if(removedCourseCarts != null)
            {
                await RemoveRange(removedCourseCarts);
            }

            return removedCourseCarts;
        }
    }
}
