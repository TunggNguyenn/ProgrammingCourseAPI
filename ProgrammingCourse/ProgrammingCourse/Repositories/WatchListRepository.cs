using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class WatchListRepository
    {
        private ProgrammingCourseDbContext programmingCourseDbContext;

        public WatchListRepository(ProgrammingCourseDbContext programmingCourseDbContext)
        {
            this.programmingCourseDbContext = programmingCourseDbContext;
        }

        public async Task<WatchList> Add(WatchList watchList)
        {
            await programmingCourseDbContext.WatchLists.AddAsync(watchList);
            await programmingCourseDbContext.SaveChangesAsync();
            return watchList;
        }

        public async Task<WatchList> Delete(int id)
        {
            var deletedWatchList = await programmingCourseDbContext.WatchLists.FindAsync(id);

            if (deletedWatchList == null)
            {
                return deletedWatchList;
            }

            programmingCourseDbContext.WatchLists.Remove(deletedWatchList);
            await programmingCourseDbContext.SaveChangesAsync();

            return deletedWatchList;
        }

        public async Task<WatchList> Get(int id)
        {
            var watchList = await programmingCourseDbContext.WatchLists.Where<WatchList>(w => w.Id == id).Include(w => w.Student).Include(w => w.Course).FirstOrDefaultAsync();
            return watchList;
        }

        public async Task<IList<WatchList>> GetAll()
        {
            var watchLists = await programmingCourseDbContext.WatchLists.Include(w => w.Student).Include(w => w.Course).ToListAsync<WatchList>();
            return watchLists;
        }

        public async Task<WatchList> Update(WatchList watchList)
        {
            if (watchList != null)
            {
                programmingCourseDbContext.WatchLists.Update(watchList);
                await programmingCourseDbContext.SaveChangesAsync();
            }

            return watchList;
        }
    }
}
