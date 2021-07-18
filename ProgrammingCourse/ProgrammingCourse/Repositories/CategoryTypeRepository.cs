using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CategoryTypeRepository : GenericRepository<CategoryType>
    {
        public CategoryTypeRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<CategoryType> GetWithAllInfoById(int id)
        {
            return await _context.Set<CategoryType>().Where(c => c.Id == id).Include(c => c.Categories).FirstOrDefaultAsync();
        }

        public async Task<List<CategoryType>> GetAllWithAllInfo()
        {
            return await _context.Set<CategoryType>().Include(c => c.Categories).ToListAsync();
        }
    }
}
