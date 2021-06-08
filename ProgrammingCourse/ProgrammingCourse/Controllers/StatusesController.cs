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


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var status = await statusRepository.Get(id);

            if (status != null)
            {
                return Ok(new
                {
                    Results = status
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await statusRepository.GetAll();
            return Ok(new
            {
                Results = statuses
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
                    Results = status
                });
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new object[] { new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } }
                });
            }
        }
    }
}
