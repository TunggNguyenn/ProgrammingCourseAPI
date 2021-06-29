using AutoMapper;
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
        private readonly StatusRepository statusRepository;
        private readonly IMapper mapper;

        public StatusesController(StatusRepository statusRepository, IMapper mapper)
        {
            this.statusRepository = statusRepository;
            this.mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await statusRepository.GetById(id);

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
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" }
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
        public async Task<IActionResult> Add([FromBody] StatusViewModel statusViewModel)
        {
            try
            {
                Status statusMapped = mapper.Map<Status>(statusViewModel);

                await statusRepository.Add(statusMapped);

                return Ok(new
                {
                    Results = statusMapped
                });

            }
            catch (Exception e)
            {
                Console.WriteLine($"ErrorMesages: {e}");

                return BadRequest(new
                {
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" }
                });
            }
        }
    }
}
