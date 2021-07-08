using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        //private readonly UserManager<User> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;

        //public UserRepository(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        //{
        //    this.userManager = userMgr;
        //    this.roleManager = roleMgr;
        //}

        //public async Task<User> GetById(string id)
        //{
        //    return await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();
        //}

        public UserRepository(ProgrammingCourseDbContext context) : base(context)
        {
        }

        public async Task<object> GetById(string id)
        {
            return await _context.Set<User>().Where<User>(usr => usr.Id == id)
                .Select(usr => new
                {
                    Id = usr.Id,
                    AvatarUrl = usr.AvatarUrl,
                    UserName = usr.UserName,
                    Email = usr.Email
                })
                .FirstOrDefaultAsync();
        }
    }
}
