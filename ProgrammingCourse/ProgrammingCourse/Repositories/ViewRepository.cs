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


        //public async Task<int> GetViewNumberInMonthByCourseId(int courseId)
        //{
        //    var view = await _context.Set<View>()
        //        .Where<View>(v => v.CourseId == courseId && v.DateTime.Month == DateTime.Now.Month && v.DateTime.Year == DateTime.Now.Year)
        //        .FirstOrDefaultAsync();
        //    return view == null ? 0 : view.Number;
        //}

        public async Task<List<dynamic>> Get10MostViewedCourseIdsInMonth()
        {
            var courses = await _context.Set<View>()
                .Where<View>(v => v.DateTime.Month == DateTime.Now.Month && v.DateTime.Year == DateTime.Now.Year)
                .GroupBy(v => v.CourseId)
                .Select(v => new
                {
                    CourseId = v.Key,
                    Sum = v.Sum(n => n.Number)
                })
                .OrderByDescending(v => v.Sum)
                .Take(10)
                .ToListAsync<dynamic>();

            return courses;
        }

        public async Task<int> GetViewNumberByCourseId(int courseId)
        {
            return await _context.Set<View>()
                .Where<View>(v => v.CourseId == courseId)
                .SumAsync(v => v.Number);
        }
    }
}
