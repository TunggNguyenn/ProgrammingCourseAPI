using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var role = await roleManager.Roles.Where<IdentityRole>(r => r.Id == id).FirstOrDefaultAsync();

            if (role != null)
            {
                return Ok(new
                {
                    Results = role
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
        public IActionResult GetAll()
        {
            return Ok(new
            {
                Results = roleManager.Roles.ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string name)
        {
            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Results = result
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
    }
}
