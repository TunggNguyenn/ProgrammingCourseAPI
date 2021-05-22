using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private StatusRepository statusRepository;

        public StatusesController(StatusRepository statusRepo)
        {
            statusRepository = statusRepo;
        }

        [HttpGet]
        public ActionResult<IList<Status>> GetAll()
        {
            var statuses = statusRepository.GetAll();
            return Ok(new
            {
                Status = true,
                Result = statuses,
                Message = new object[] { new { Code = "Success", Description = "Success!" } }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StatusViewModel statusViewModel)
        {
            Status status = new Status() { Name = statusViewModel.Name };

            var result = await statusRepository.Add(status);

            if (result != null)
            {
                return Ok(new
                {
                    Status = true,
                    Result = status,
                    Message = new object[] { new { Code = "Success", Description = "Success!" } }
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = new object[] { new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } }
                });
            }
        }
    }
}
