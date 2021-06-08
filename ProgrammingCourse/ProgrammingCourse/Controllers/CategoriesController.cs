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
    public class CategoriesController : ControllerBase
    {
        private CategoryRepository categoryRepository;
        private CourseRepository courseRepository;

        public CategoriesController(CategoryRepository categoryRepo, CourseRepository courseRepo)
        {
            categoryRepository = categoryRepo;
            courseRepository = courseRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await categoryRepository.Get(id);

            if(category != null)
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
                    Errors = new object[] { new { Code = "InvalidId", Description = "Invalid Id!" } }
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
        public async Task<IActionResult> Create([FromForm] CategoryViewModel categoryViewModel)
        {
            Category category = new Category() { Name = categoryViewModel.Name, CategoryTypeId = categoryViewModel.CategoryTypeId };

            var result = await categoryRepository.Add(category);

            if (result != null)
            {
                return Ok(new
                {
                    Results = result
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


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CategoryViewModel categoryViewModel)
        {
            var updatedCategory = await categoryRepository.Get(categoryViewModel.Id);

            if (updatedCategory != null)
            {
                updatedCategory.Name = categoryViewModel.Name;
                updatedCategory.CategoryTypeId = categoryViewModel.CategoryTypeId;

                var result = await categoryRepository.Update(updatedCategory);
                
                if(result != null)
                {
                    return Ok(new
                    {
                        Results = result
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
            else
            {
                return BadRequest(new
                {
                    Errors = new object[] { new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var courses = await courseRepository.GetByCategoryId(id);

            if(courses.Count > 0)
            {
                return BadRequest(new
                {
                    Errors = new object[] { new { Code = "ExistedCourse", Description = "Category has already existed course" } }
                });
            }

            var deletedCategory = await categoryRepository.Delete(id);

            if (deletedCategory != null)
            {
                return Ok(new
                {
                    Results = deletedCategory
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


        [HttpGet()]
        [Route("MostRegisteredCategories")]
        public async Task<IActionResult> MostRegisteredCategories()
        {
            var mostRegisteredCategories = await categoryRepository.GetMostRegisteredCategories();

            return Ok(new
            {
                Results = mostRegisteredCategories
            });
        }
    }
}
