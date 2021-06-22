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
            var courses = await courseRepository.GetAll();
            return Ok(new
            {
                Results = courses
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
                    Results = result
                });
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
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
                        Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
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
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
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
                Results = outstandingCourses
            });
        }

        [HttpGet()]
        [Route("MostViewedCourses")]
        public async Task<IActionResult> MostViewedCourses()
        {
            var mostViewedCourses = await courseRepository.GetMostViewedCourses();

            return Ok(new
            {
                Results = mostViewedCourses
            });
        }


        [HttpGet()]
        [Route("NewestCourses")]
        public async Task<IActionResult> NewestCourses()
        {
            var newestCourses = await courseRepository.GetNewestCourses();

            return Ok(new
            {
                Results = newestCourses
            });
        }


        [HttpGet()]
        [Route("BestSellerCoursesByCategoryId")]
        public async Task<IActionResult> BestSellerCoursesByCategoryId([FromQuery] int courseId, [FromQuery] int categoryId)
        {
            var bestSellerCourses = await courseRepository.GetBestSellerCoursesByCategoryId(courseId, categoryId);

            return Ok(new
            {
                Results = bestSellerCourses
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
                        Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
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
                Results = courses
            });
        }


        [HttpGet()]
        [Route("BestSellerCoursesByCategoryTypeId")]
        public async Task<IActionResult> BestSellerCoursesByCategoryTypeId([FromQuery] int categoryTypeId, [FromQuery] int pageSize, [FromQuery] int pageOffset)
        {
            var bestSellerCourses = await courseRepository.GetBestSellerCoursesByCategoryTypeId(categoryTypeId, pageSize, pageOffset);

            return Ok(new
            {
                Results = bestSellerCourses
            });
        }


        [HttpGet()]
        [Route("OutstandingCoursesByCategoryId")]
        public async Task<IActionResult> OutstandingCoursesByCategoryId([FromQuery] int categoryId, [FromQuery] int pageSize, [FromQuery] int pageOffset)
        {
            var outstandingCourses = await courseRepository.GetOutStandingCoursesByCategoryId(categoryId, pageSize, pageOffset);

            return Ok(new
            {
                Results = outstandingCourses
            });
        }


        //For testing
        [HttpPost]
        [Route("CreateRange")]
        public async Task<IActionResult> CreateRange([FromBody] IList<CourseViewModel> courseViewModel)
        {
            IList<Course> courses = new List<Course>();

            foreach(var c in courseViewModel)
            {
                Course course = new Course()
                {
                    Name = c.Name,
                    CategoryId = c.CategoryId,
                    LecturerId = c.LecturerId,
                    ImageUrl = c.ImageUrl,
                    Price = c.Price,
                    Discount = c.Discount,
                    View = c.View,
                    ShortDiscription = c.ShortDiscription,
                    DetailDiscription = c.DetailDiscription,
                    LastUpdated = DateTime.Now,
                    StatusId = c.StatusId
                };

                courses.Add(course);
            }
            

            var result = await courseRepository.AddRange(courses);

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
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" }
                });
            }
        }
    }
}
