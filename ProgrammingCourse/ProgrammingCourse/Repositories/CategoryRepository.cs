using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CategoryRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public CategoryRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<Category> Add(Category category)
        {
            await programmingCourseDbContext.Categories.AddAsync(category);
            await programmingCourseDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> Delete(int id)
        {
            var deletedCategory = await programmingCourseDbContext.Categories.FindAsync(id);

            if (deletedCategory == null)
            {
                return deletedCategory;
            }

            programmingCourseDbContext.Categories.Remove(deletedCategory);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedCategory;
        }

        public async Task<Category> Get(int id)
        {
            var category = await programmingCourseDbContext.Categories.Where<Category>(c => c.Id == id).Include(c => c.Courses).Include(c => c.CategoryType).FirstOrDefaultAsync();
            return category;
        }

        public async Task<IList<Category>> GetAll()
        {
            var categories = await programmingCourseDbContext.Categories.ToListAsync<Category>();
            return categories;
        }

        public async Task<Category> Update(Category category)
        {
            if (category != null)
            {
                programmingCourseDbContext.Categories.Update(category);
                await programmingCourseDbContext.SaveChangesAsync();
            }

            return category;
        }


        public async Task<dynamic> GetMostRegisteredCategories()
        {
            var categories = await programmingCourseDbContext.StudentCourses
                .Include(s => s.Course).ThenInclude(s => s.Category)
                .GroupBy(s => new
                {
                    CategoryId = s.Course.Category.Id,
                    CategoryName = s.Course.Category.Name
                })
                .Select(s => new
                {
                    CategoryId = s.Key.CategoryId,
                    CategoryName = s.Key.CategoryName,
                    TotalRegisteredUserNumber = s.Count()
                })
                .OrderByDescending(s => s.TotalRegisteredUserNumber)
                .ToListAsync<dynamic>();
            return categories;
        }
    }
}
