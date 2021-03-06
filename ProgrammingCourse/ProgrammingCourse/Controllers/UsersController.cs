using AutoMapper;
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
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public UsersController(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr, IMapper mapper)
        {
            this.userManager = userMgr;
            this.roleManager = roleMgr;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();

            if (user != null)
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

            if (users.Count == 0)
            {
                return Ok(new
                {
                    Results = new object[] { }
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


        [HttpGet]
        [Route("GetStudentList")]
        public async Task<IActionResult> GetStudentList([FromQuery] PaginationParameters paginationParameters)
        {
            var users = await userManager.Users.ToListAsync();

            if (users.Count == 0)
            {
                return Ok(new
                {
                    Results = new object[] { }
                });
            }

            IList<object> objectUsers = new List<object>();

            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);

                if(role[0] == "Student")
                {
                    dynamic dynamicUsers = new ExpandoObject();
                    dynamicUsers.Info = user;
                    dynamicUsers.Role = role[0];

                    objectUsers.Add(dynamicUsers);
                }    
            }

            paginationParameters.TotalItems = objectUsers.Count;
            paginationParameters.TotalPages = Convert.ToInt32(Math.Ceiling((double)objectUsers.Count / paginationParameters.PageSize));
            paginationParameters.PageNumber = (paginationParameters.PageNumber > paginationParameters.TotalPages) ? paginationParameters.TotalPages : paginationParameters.PageNumber;

            return Ok(new
            {
                Results = new 
                {
                    Students = objectUsers.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize).Take(paginationParameters.PageSize),
                    PaginationStatus = paginationParameters
                }
            });
        }


        [HttpGet]
        [Route("GetLecturerList")]
        public async Task<IActionResult> GetLecturerList([FromQuery] PaginationParameters paginationParameters)
        {
            var users = await userManager.Users.ToListAsync();

            if (users.Count == 0)
            {
                return Ok(new
                {
                    Results = (object[])null
                });
            }

            IList<object> objectUsers = new List<object>();

            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);

                if (role[0] == "Lecturer")
                {
                    dynamic dynamicUsers = new ExpandoObject();
                    dynamicUsers.Info = user;
                    dynamicUsers.Role = role[0];

                    objectUsers.Add(dynamicUsers);
                }
            }

            paginationParameters.TotalItems = objectUsers.Count;
            paginationParameters.TotalPages = Convert.ToInt32(Math.Ceiling((double)objectUsers.Count / paginationParameters.PageSize));
            paginationParameters.PageNumber = (paginationParameters.PageNumber > paginationParameters.TotalPages) ? paginationParameters.TotalPages : paginationParameters.PageNumber;

            return Ok(new
            {
                Results = new
                {
                    Lecturers = objectUsers.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize).Take(paginationParameters.PageSize),
                    PaginationStatus = paginationParameters
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var user = await userManager.Users.Where<User>(c => c.Id == id).FirstOrDefaultAsync();


            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);

                if (role.Count > 0)
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
        public async Task<IActionResult> Lock([FromQuery] string id)
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
        public async Task<IActionResult> Unlock([FromQuery] string id)
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
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel updateUserViewModel)
        {
            var updatedUser = await userManager.Users.Where<User>(c => c.Id == updateUserViewModel.UserId).FirstOrDefaultAsync();

            if (updatedUser != null)
            {

                if(updateUserViewModel.NewUserName != null)
                {
                    updatedUser.UserName = updateUserViewModel.NewUserName;
                }

                if(updateUserViewModel.NewAvatarUrl != null)
                {
                    updatedUser.AvatarUrl = updateUserViewModel.NewAvatarUrl;
                }

                if(updateUserViewModel.NewEmail != null)
                {
                    //Check whether email is existed
                    bool isExisted = await EmailChecker.Check(updateUserViewModel.NewEmail);
                    if (isExisted == false)
                    {
                        return BadRequest(new
                        {
                            Errors = new { Code = "NotExistedEmailAddress", Description = "Email address is not existed!" }
                        });
                    }
                    else
                    {
                        updatedUser.Email = updateUserViewModel.NewEmail;
                    }
                }

                if (updateUserViewModel.Description != null)
                {
                    updatedUser.Description = updateUserViewModel.Description;
                }


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
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleViewModel changeRoleViewModel)
        {
            var user = await userManager.Users.Where<User>(c => c.Id == changeRoleViewModel.UserId).FirstOrDefaultAsync();

            //Check IsRole existed
            IdentityRole isRoleExisted = await roleManager.FindByNameAsync(changeRoleViewModel.RoleName);

            if (isRoleExisted == null)
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidRole", Description = $"Role {changeRoleViewModel.RoleName} is invalid!" }
                });
            }

            if (user != null)
            {
                var currentRoleList = await userManager.GetRolesAsync(user);

                await userManager.RemoveFromRoleAsync(user, currentRoleList[0]);
                await userManager.AddToRoleAsync(user, changeRoleViewModel.RoleName);

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
