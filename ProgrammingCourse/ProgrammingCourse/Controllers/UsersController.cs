using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            roleManager = roleMgr;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();

            if(user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                dynamic dynamicUser = new ExpandoObject();
                dynamicUser.Info = user;
                dynamicUser.Role = role[0];

                return Ok(new
                {
                    Results = dynamicUser
                });
            }

            return BadRequest(new
            {
                Errors = new object[] { new { Code = "InvalidId", Description = "Invalid Id!" } }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await userManager.Users.ToListAsync();

            if(users.Count == 0)
            {
                return Ok(new
                {
                    Results = new object[] {}
                });
            }

            IList<object> objectUsers = new List<object>();

            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);
                dynamic dynamicUsers = new ExpandoObject();
                dynamicUsers.Info = user;
                dynamicUsers.Role = role[0];

                objectUsers.Add(dynamicUsers);
            }

            return Ok(new
            {
                Results = objectUsers
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();


            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);

                if(role.Count > 0)
                {
                    await userManager.RemoveFromRoleAsync(user, role[0]);
                }
 

                var deletedUser = await userManager.DeleteAsync(user);

                return Ok(new
                {
                    Results = deletedUser
                });
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new object[] { new { Code = "InvalidId", Description = "Invalid Id!" } }
                });
            }
        }
    }
}
