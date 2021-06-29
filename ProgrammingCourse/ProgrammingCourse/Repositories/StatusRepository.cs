using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class StatusRepository : GenericRepository<Status>
    {
        public StatusRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }
    }
}
