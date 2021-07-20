using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using ProgrammingCourse.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers0
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService categoryService;
        private readonly CourseService courseService;
        private readonly IMapper mapper;

        public CategoriesController(CategoryService categoryService, CourseService courseService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWithAllInfoById(int id)
        {
            var category = await categoryService.GetWithAllInfoById(id);

            if (category != null)
            {
                return Ok(new
                {
                    Results = category
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
        [Route("GetWithAllInfoByName")]
        public async Task<IActionResult> GetWithAllInfoByName([FromQuery] string name)
        {
            var category = await categoryService.GetWithAllInfoByName(name);

            if (category != null)
            {
                return Ok(new
                {
                    Results = category
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
        [Route("CategoryListByCategoryTypeId")]
        public async Task<IActionResult> CategoryListByCategoryTypeId([FromQuery] int categoryId)
        {
            var categoryList = await categoryService.GetCategoryListByCategoryTypeId(categoryId);

            if (categoryList.Count != 0)
            {
                return Ok(new
                {
                    Results = categoryList
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
            var categories = await categoryService.GetAll();

            return Ok(new
            {
                Results = categories
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryViewModel categoryViewModel)
        {
            try
            {
                Category categoryMapped = mapper.Map<Category>(categoryViewModel);

                await categoryService.Add(categoryMapped);

                return Ok(new
                {
                    Results = categoryMapped
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
        public async Task<IActionResult> Update([FromBody] CategoryViewModel categoryViewModel)
        {
            try
            {
                Category categoryMapped = mapper.Map<Category>(categoryViewModel);

                await categoryService.Update(categoryMapped);

                return Ok(new
                {
                    Results = categoryMapped
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
                var courses = await courseService.GetByCategoryId(id);

                if (courses.Count > 0)
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "ExistedCourse", Description = "Category has already existed course" }
                    });
                }

                var removedCategory = await categoryService.GetById(id);

                await categoryService.Remove(removedCategory);

                return Ok(new
                {
                    Results = removedCategory
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


        [HttpGet()]
        [Route("MostRegisteredCategories")]
        public async Task<IActionResult> MostRegisteredCategories()
        {
            var mostRegisteredCategories = await categoryService.GetMostRegisteredCategories();

            return Ok(new
            {
                Results = mostRegisteredCategories
            });
        }
    }
}
