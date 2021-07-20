using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {

        public CategoryRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<Category> GetWithAllInfoById(int id)
        {
            return await _context.Set<Category>().Where(c => c.Id == id).Include(c => c.CategoryType).Include(c => c.Courses).FirstOrDefaultAsync();
        }

        public async Task<IList<dynamic>> GetCategoryListByCategoryTypeId(int categoryId)
        {
            return await _context.Set<Category>()
                .Where(c => c.CategoryTypeId == categoryId)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Label = c.Label,
                    ImageUrl = c.ImageUrl,
                    CategoryTypeId = c.CategoryTypeId,
                    CategoryTypeName = c.CategoryType.Name,
                    CourseNumber = c.Courses.Count
                })
                .ToListAsync<dynamic>();
        }


        public async Task<Category> GetWithAllInfoByName(string name)
        {
            return await _context.Set<Category>().Where(c => c.Name == name).Include(c => c.CategoryType).Include(c => c.Courses).FirstOrDefaultAsync();
        }


        public async Task<dynamic> GetMostRegisteredCategories()
        {
            var categories = await _context.StudentCourses
                .Include(s => s.Course).ThenInclude(s => s.Category)
                .GroupBy(s => new
                {
                    CategoryId = s.Course.Category.Id,
                    CategoryName = s.Course.Category.Name,
                    ImageUrl = s.Course.Category.ImageUrl,
                    Label = s.Course.Category.Label
                })
                .Select(s => new
                {
                    CategoryId = s.Key.CategoryId,
                    CategoryName = s.Key.CategoryName,
                    ImageUrl = s.Key.ImageUrl,
                    Label = s.Key.Label,
                    TotalRegisteredUserNumber = s.Count()
                })
                .OrderByDescending(s => s.TotalRegisteredUserNumber)
                .ToListAsync<dynamic>();
            return categories;
        }

        //public async Task<List<Category>> GetByCategoryTypeId(int categoryTypeId)
        //{
        //    return await _context.Set<Category>().Where(c => c.CategoryTypeId == categoryTypeId).ToListAsync();
        //}
    }
}
