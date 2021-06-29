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
    public class CategoryTypesController : ControllerBase
    {
        private readonly CategoryTypeRepository categoryTypeRepository;
        private readonly IMapper mapper;

        public CategoryTypesController(CategoryTypeRepository categoryTypeRepository, IMapper mapper)
        {
            this.categoryTypeRepository = categoryTypeRepository;
            this.mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoryType = await categoryTypeRepository.GetById(id);

            if (categoryType != null)
            {
                return Ok(new
                {
                    Results = categoryType
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
            var categoryTypes = await categoryTypeRepository.GetAll();

            return Ok(new
            {
                Results = categoryTypes
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryTypeViewModel categoryTypeViewModel)
        {
            try
            {
                CategoryType categoryTypeMapped = mapper.Map<CategoryType>(categoryTypeViewModel);

                await categoryTypeRepository.Add(categoryTypeMapped);

                return Ok(new
                {
                    Results = categoryTypeMapped
                });

            }
            catch(Exception e)
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
