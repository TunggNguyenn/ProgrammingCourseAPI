using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository categoryRepository;
        private readonly CourseRepository courseRepository;
        private readonly IMapper mapper;

        public CategoriesController(CategoryRepository categoryRepository, CourseRepository courseRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await categoryRepository.GetById(id);

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
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryRepository.GetAll();

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

                await categoryRepository.Add(categoryMapped);

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

                await categoryRepository.Update(categoryMapped);

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
                var courses = await courseRepository.GetByCategoryId(id);

                if (courses.Count > 0)
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "ExistedCourse", Description = "Category has already existed course" }
                    });
                }

                var removedCategory = await categoryRepository.GetById(id);

                await categoryRepository.Remove(removedCategory);

                return Ok(new
                {
                    Results = "deletedCategory"
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


        //[HttpGet()]
        //[Route("MostRegisteredCategories")]
        //public async Task<IActionResult> MostRegisteredCategories()
        //{
        //    var mostRegisteredCategories = await categoryRepository.GetMostRegisteredCategories();

        //    return Ok(new
        //    {
        //        Results = mostRegisteredCategories
        //    });
        //}
    }
}
