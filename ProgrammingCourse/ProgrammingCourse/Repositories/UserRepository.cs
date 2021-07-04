using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public class UserRepository
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserRepository(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            this.userManager = userMgr;
            this.roleManager = roleMgr;
        }

        public async Task<User> GetById(string id)
        {
            return await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
