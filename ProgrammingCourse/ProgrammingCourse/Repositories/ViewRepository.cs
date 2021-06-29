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
    }
}
