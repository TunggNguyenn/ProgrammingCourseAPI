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
            var view = await viewRepository.GetById(id);

            if (view != null)
            {
                return Ok(new
                {
                    Results = view
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
            var views = await viewRepository.GetAll();
            return Ok(new
            {
                Results = views
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ViewViewModel viewViewModel)
        {
            try
            {
                View viewMapped = mapper.Map<View>(viewViewModel);
                viewMapped.Number = 1;

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
                viewMapped.Number += 1;

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


        [HttpPut]
        [Route("UpdateView")]
        public async Task<IActionResult> UpdateView([FromBody] ViewViewModel viewViewModel)
        {
            try
            {
                var view = await viewRepository.GetByCouseIdAndDateTime(viewViewModel.CourseId, viewViewModel.DateTime);

                if(view == null)
                {
                    View viewMapped = mapper.Map<View>(viewViewModel);
                    viewMapped.Number = 1;

                    await viewRepository.Add(viewMapped);

                    return Ok(new
                    {
                        Results = viewMapped
                    });
                }
                else
                {
                    view.Number += 1;
                    await viewRepository.Update(view);

                    return Ok(new
                    {

                        Results = view
                    });
                }
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
