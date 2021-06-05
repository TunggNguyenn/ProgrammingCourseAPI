using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CategoryTypeRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public CategoryTypeRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<CategoryType> Get(int id)
        {
            var categoryType = await programmingCourseDbContext.CategoryTypes.Where<CategoryType>(c => c.Id == id).Include(c => c.Categories).FirstOrDefaultAsync();
            return categoryType;
        }

        public async Task<CategoryType> Add(CategoryType categoryType)
        {
            await programmingCourseDbContext.CategoryTypes.AddAsync(categoryType);
            await programmingCourseDbContext.SaveChangesAsync();
            return categoryType;
        }

        public async Task<CategoryType> Delete(int id)
        {
            var deletedCategoryType = await programmingCourseDbContext.CategoryTypes.FindAsync(id);

            if (deletedCategoryType == null)
            {
                return deletedCategoryType;
            }

            programmingCourseDbContext.CategoryTypes.Remove(deletedCategoryType);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedCategoryType;
        }

        public async Task<IList<CategoryType>> GetAll()
        {
            var categoryTypes = await programmingCourseDbContext.CategoryTypes.Include(c => c.Categories).ToListAsync<CategoryType>();
            return categoryTypes;
        }

        public async Task<CategoryType> Update(CategoryType categoryType)
        {
            programmingCourseDbContext.CategoryTypes.Update(categoryType);
            await programmingCourseDbContext.SaveChangesAsync();

            return categoryType;
        }
    }
}
