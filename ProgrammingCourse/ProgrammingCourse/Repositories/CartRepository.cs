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

        public async Task<Cart> GetByStudentId(string studentId)
        {
            return await _context.Set<Cart>().Where(c => c.StudentId == studentId).FirstOrDefaultAsync();
        }

        public async Task<Cart> GetWithCourseCartListByStudentId(string studentId)
        {
            var cart =  await _context.Set<Cart>().Where(c => c.StudentId == studentId).FirstOrDefaultAsync();

            if(cart != null)
            {
                var courseCarts = await _context.Set<CourseCart>().Where(cc => cc.CartId == cart.Id).Include(cc => cc.Course).ToListAsync();
                cart.CourseCarts = courseCarts;
            }    

            return cart;
        }
    }
}
