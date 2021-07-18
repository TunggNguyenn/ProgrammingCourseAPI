using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using ProgrammingCourse.Services;
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
        private readonly CategoryTypeService categoryTypeService;
        private readonly IMapper mapper;

        public CategoryTypesController(CategoryTypeService categoryTypeService, IMapper mapper)
        {
            this.categoryTypeService = categoryTypeService;
            this.mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetWithAllInfoById(int id)
        {
            var categoryType = await categoryTypeService.GetWithAllInfoById(id);

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
        [Route("GetFormattedCategoryTypeById")]
        public async Task<IActionResult> GetFormattedCategoryTypeById([FromQuery] int id)
        {
            var formattedCategoryType = await categoryTypeService.GetFormattedCategoryTypeById(id);


            if (formattedCategoryType != null)
            {
                return Ok(new
                {
                    Results = formattedCategoryType
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
            var categoryTypes = await categoryTypeService.GetAllWithAllInfo();

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

                await categoryTypeService.Add(categoryTypeMapped);

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
