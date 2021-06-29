using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class LectureRepository : GenericRepository<Lecture>
    {
        public LectureRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }



        //public async Task<IList<Lecture>> GetAllByCourseId(int courseId)
        //{
        //    var lectures = await programmingCourseDbContext.Lectures
        //        .Where<Lecture>(l => l.CourseId == courseId)
        //        .ToListAsync<Lecture>();
        //    return lectures;
        //}
    }
}
