using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CartRepository : GenericRepository<Cart>
    {
        public CartRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<List<Cart>> GetCartListByStudentId(string studentId)
        {
            return await _context.Set<Cart>().Where(c => c.StudentId == studentId).ToListAsync();
        }

        public async Task<Cart> GetByCourseIdAndStudentId(int courseId, string studentId)
        {
            return await _context.Set<Cart>().Where(c => c.CourseId == courseId && c.StudentId == studentId).FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetCourseIdListByStudentId(string studentId)
        {
            return  await _context.Set<Cart>().Where(c => c.StudentId == studentId)
                .Select(c => c.CourseId)
                .ToListAsync();
        }
    }
}
