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
        private CategoryTypeRepository categoryTypeRepository;

        public CategoryTypesController(CategoryTypeRepository categoryTypeRepo)
        {
            categoryTypeRepository = categoryTypeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categoryTypes = await categoryTypeRepository.GetAll();
            return Ok(new
            {
                Results = categoryTypes,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryTypeViewModel categoryTypeViewModel)
        {
            CategoryType categoryType = new CategoryType() { Name = categoryTypeViewModel.Name };

            var result = await categoryTypeRepository.Add(categoryType);

            if (result != null)
            {
                return Ok(new
                {
                    Results = result,
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
