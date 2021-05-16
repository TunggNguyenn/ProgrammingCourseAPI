using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        [HttpGet]
        public ActionResult<IList<IdentityRole>> GetAll()
        {
            return roleManager.Roles.ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string name)
        {
            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Result = result,
                    Message = "Role Adding Successful!"
                });
            }
            else
            {
                return Ok(new
                {
                    Result = result,
                    Message = "Role Adding Unsuccessful!"
                });
            }
        }
    }
}
