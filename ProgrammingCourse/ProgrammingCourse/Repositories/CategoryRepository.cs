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


        //public async Task<dynamic> GetMostRegisteredCategories()
        //{
        //    var categories = await context.StudentCourses
        //        .Include(s => s.Course).ThenInclude(s => s.Category)
        //        .GroupBy(s => new
        //        {
        //            CategoryId = s.Course.Category.Id,
        //            CategoryName = s.Course.Category.Name
        //        })
        //        .Select(s => new
        //        {
        //            CategoryId = s.Key.CategoryId,
        //            CategoryName = s.Key.CategoryName,
        //            TotalRegisteredUserNumber = s.Count()
        //        })
        //        .OrderByDescending(s => s.TotalRegisteredUserNumber)
        //        .ToListAsync<dynamic>();
        //    return categories;
        //}
    }
}
