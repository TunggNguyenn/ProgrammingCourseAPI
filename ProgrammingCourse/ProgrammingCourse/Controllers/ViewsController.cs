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

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsController : ControllerBase
    {
        private readonly ViewRepository viewRepository;
        private readonly IMapper mapper;

        public ViewsController(ViewRepository viewRepository, IMapper mapper)
        {
            this.viewRepository = viewRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await viewRepository.GetById(id);

            if (course != null)
            {
                return Ok(new
                {
                    Results = course
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
            var courses = await viewRepository.GetAll();
            return Ok(new
            {
                Results = courses
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ViewViewModel viewViewModel)
        {
            try
            {
                View viewMapped = mapper.Map<View>(viewViewModel);
                viewMapped.DateTime = DateTime.Now;

                await viewRepository.Add(viewMapped);

                return Ok(new
                {
                    Results = viewMapped
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


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ViewViewModel viewViewModel)
        {
            try
            {
                View viewMapped = mapper.Map<View>(viewViewModel);
                viewMapped.DateTime = DateTime.Now;

                await viewRepository.Update(viewMapped);

                return Ok(new
                {

                    Results = viewMapped
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var removedView = await viewRepository.GetById(id);

                await viewRepository.Remove(removedView);

                return Ok(new
                {
                    Results = removedView
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"ErrorMesages: {e}");

                return BadRequest(new
                {
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" }
                });
            }
        }
    }
}
