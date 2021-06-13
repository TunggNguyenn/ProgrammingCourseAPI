using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
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
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
                });
            }
        }


        [HttpGet]
        [Route("LockUser")]
        public async Task<IActionResult> Lock([Required] string id)
        {
            var lockedUser = await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();


            if (lockedUser != null)
            {
                lockedUser.IsLocked = true;
                await userManager.UpdateAsync(lockedUser);

                return Ok(new
                {
                    Results = lockedUser
                });
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
                });
            }
        }


        [HttpGet]
        [Route("UnlockUser")]
        public async Task<IActionResult> Unlock([Required] string id)
        {
            var unlockedUser = await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();


            if (unlockedUser != null)
            {
                unlockedUser.IsLocked = false;
                await userManager.UpdateAsync(unlockedUser);

                return Ok(new
                {
                    Results = unlockedUser
                });
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
                });
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromForm][Required] string userId, [FromForm][Required] string newUserName, [FromForm][Required] string newAvatarUrl, [FromForm][Required] string newEmail)
        {
            var updatedUser = await userManager.Users.Where<User>(c => c.Id == userId).FirstOrDefaultAsync();

            if (updatedUser != null)
            {
                //Check whether email is existed
                bool isExisted = await EmailChecker.Check(newEmail);
                if (isExisted == false)
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "NotExistedEmailAddress", Description = "Email address is not existed!" } 
                    });
                }

                updatedUser.UserName = newUserName;
                updatedUser.AvatarUrl = newAvatarUrl;
                updatedUser.Email = newEmail;

                var result = await userManager.UpdateAsync(updatedUser);

                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Results = updatedUser
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = result.Errors.ToArray()[0]
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
                });
            }
        }


        [HttpPut]
        [Route("ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromForm] string userId, [FromForm] string roleName)
        {
            var user = await userManager.Users.Where<User>(c => c.Id == userId).FirstOrDefaultAsync();

            //Check IsRole existed
            IdentityRole isRoleExisted = await roleManager.FindByNameAsync(roleName);

            if (isRoleExisted == null)
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidRole", Description = $"Role {roleName} is invalid!" } 
                });
            }

            if (user != null)
            {
                var currentRoleList = await userManager.GetRolesAsync(user);

                await userManager.RemoveFromRoleAsync(user, currentRoleList[0]);
                await userManager.AddToRoleAsync(user, roleName);

                return Ok(new
                {
                    Results = user
                });
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
                });
            }
        }
    }
}
