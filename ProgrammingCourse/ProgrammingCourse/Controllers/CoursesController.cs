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
    public class CoursesController : ControllerBase
    {
        private CourseRepository courseRepository;

        public CoursesController(CourseRepository courseRepo)
        {
            courseRepository = courseRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await courseRepository.Get(id);

            if (course != null)
            {
                return Ok(new
                {
                    Results = course,
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
            var courses = await courseRepository.GetAll();
            return Ok(new
            {
                Results = courses,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseViewModel courseViewModel)
        {
            Course course = new Course()
            {
                Name = courseViewModel.Name,
                CategoryId = courseViewModel.CategoryId,
                LecturerId = courseViewModel.LecturerId,
                ImageUrl = courseViewModel.ImageUrl,
                Price = courseViewModel.Price,
                Discount = courseViewModel.Discount,
                View = courseViewModel.View,
                ShortDiscription = courseViewModel.ShortDiscription,
                DetailDiscription = courseViewModel.DetailDiscription,
                LastUpdated = DateTime.Now,
                StatusId = courseViewModel.StatusId
            };

            var result = await courseRepository.Add(course);

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


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CourseViewModel courseViewModel)
        {
            var updatedCourse = await courseRepository.Get(courseViewModel.Id);

            if (updatedCourse != null)
            {
                updatedCourse.Name = courseViewModel.Name;
                updatedCourse.CategoryId = courseViewModel.CategoryId;
                updatedCourse.LecturerId = courseViewModel.LecturerId;
                updatedCourse.ImageUrl = courseViewModel.ImageUrl;
                updatedCourse.Price = courseViewModel.Price;
                updatedCourse.Discount = courseViewModel.Discount;
                updatedCourse.View = courseViewModel.View;
                updatedCourse.ShortDiscription = courseViewModel.ShortDiscription;
                updatedCourse.DetailDiscription = courseViewModel.DetailDiscription;
                updatedCourse.LastUpdated = DateTime.Now;
                updatedCourse.StatusId = courseViewModel.StatusId;

                var result = await courseRepository.Update(updatedCourse);

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
            var deletedCourse = await courseRepository.Delete(id);

            if (deletedCourse != null)
            {
                return Ok(new
                {
                    Results = deletedCourse
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
        [Route("OutstandingCourses")]
        public async Task<IActionResult> OutstandingCourses()
        {
            var outstandingCourses = await courseRepository.GetOutStandingCourses();

            return Ok(new
            {
                Results = outstandingCourses,
            });
        }

        [HttpGet()]
        [Route("MostViewedCourses")]
        public async Task<IActionResult> MostViewedCourses()
        {
            var mostViewedCourses = await courseRepository.GetMostViewedCourses();

            return Ok(new
            {
                Results = mostViewedCourses,
            });
        }


        [HttpGet()]
        [Route("NewestCourses")]
        public async Task<IActionResult> NewestCourses()
        {
            var newestCourses = await courseRepository.GetNewestCourses();

            return Ok(new
            {
                Results = newestCourses,
            });
        }


        [HttpGet()]
        [Route("BestSellerCoursesByCategoryId")]
        public async Task<IActionResult> BestSellerCoursesByCategoryId([FromQuery] int courseId, [FromQuery] int categoryId)
        {
            var bestSellerCourses = await courseRepository.GetBestSellerCoursesByCategoryId(courseId, categoryId);

            return Ok(new
            {
                Results = bestSellerCourses,
            });
        }


        [HttpPut]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromForm] int courseId, [FromForm] int statusId)
        {
            var updatedCourse = await courseRepository.Get(courseId);

            if (updatedCourse != null)
            {
                updatedCourse.StatusId = statusId;

                var result = await courseRepository.Update(updatedCourse);

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
            else
            {
                return BadRequest(new
                {
                    Errors = new object[] { new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } }
                });
            }
        }



        [HttpGet]
        [Route("GetAllByLecturerId")]
        public async Task<IActionResult> GetAllByLecturerId([FromQuery] string lecturerId)
        {
            var courses = await courseRepository.GetAllByLecturerId(lecturerId);
            return Ok(new
            {
                Results = courses,
            });
        }
    }
}
