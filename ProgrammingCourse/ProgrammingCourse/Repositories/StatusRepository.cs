using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class StatusRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public StatusRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<Status> Add(Status status)
        {
            await programmingCourseDbContext.Statuses.AddAsync(status);
            await programmingCourseDbContext.SaveChangesAsync();
            return status;
        }

        public async Task<Status> Delete(int id)
        {
            var deletedStatus = await programmingCourseDbContext.Statuses.FindAsync(id);

            if (deletedStatus == null)
            {
                return deletedStatus;
            }

            programmingCourseDbContext.Statuses.Remove(deletedStatus);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedStatus;
        }

        public Status Get(int id)
        {
            var status = programmingCourseDbContext.Statuses.Where<Status>(s => s.Id == id).FirstOrDefault();
            return status;
        }

        public IList<Status> GetAll()
        {
            var statuses = programmingCourseDbContext.Statuses.ToList<Status>();
            return statuses;
        }

        public async Task<Status> Update(Status status)
        {
            programmingCourseDbContext.Statuses.Update(status);
            await programmingCourseDbContext.SaveChangesAsync();

            return status;
        }
    }
}
