﻿using AutoMapper;
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
        private readonly CourseRepository courseRepository;
        private readonly IMapper mapper;

        public CoursesController(CourseRepository courseRepository, IMapper mapper)
        {
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await courseRepository.GetById(id);

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
        public async Task<IActionResult> Add([FromBody] CourseViewModel courseViewModel)
        {
            try
            {
                Course courseMapped = mapper.Map<Course>(courseViewModel);
                courseMapped.LastUpdated = DateTime.Now;

                await courseRepository.Add(courseMapped);

                return Ok(new
                {
                    Results = courseMapped
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


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseViewModel courseViewModel)
        {
            try
            {
                Course courseMapped = mapper.Map<Course>(courseViewModel);
                courseMapped.LastUpdated = DateTime.Now;

                await courseRepository.Update(courseMapped);

                return Ok(new
                {

                    Results = courseMapped
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var removedCourse = await courseRepository.GetById(id);

                await courseRepository.Remove(removedCourse);

                return Ok(new
                {
                    Results = "deletedCourse"
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
        //[Route("OutstandingCourses")]
        //public async Task<IActionResult> OutstandingCourses()
        //{
        //    var outstandingCourses = await courseRepository.GetOutStandingCourses();

        //    return Ok(new
        //    {
        //        Results = outstandingCourses
        //    });
        //}

        //[HttpGet()]
        //[Route("MostViewedCourses")]
        //public async Task<IActionResult> MostViewedCourses()
        //{
        //    var mostViewedCourses = await courseRepository.GetMostViewedCourses();

        //    return Ok(new
        //    {
        //        Results = mostViewedCourses
        //    });
        //}


        //[HttpGet()]
        //[Route("NewestCourses")]
        //public async Task<IActionResult> NewestCourses()
        //{
        //    var newestCourses = await courseRepository.GetNewestCourses();

        //    return Ok(new
        //    {
        //        Results = newestCourses
        //    });
        //}


        //[HttpGet()]
        //[Route("BestSellerCoursesByCategoryId")]
        //public async Task<IActionResult> BestSellerCoursesByCategoryId([FromQuery] int courseId, [FromQuery] int categoryId)
        //{
        //    var bestSellerCourses = await courseRepository.GetBestSellerCoursesByCategoryId(courseId, categoryId);

        //    return Ok(new
        //    {
        //        Results = bestSellerCourses
        //    });
        //}


        //[HttpPut]
        //[Route("ChangeStatus")]
        //public async Task<IActionResult> ChangeStatus([FromForm] int courseId, [FromForm] int statusId)
        //{
        //    var updatedCourse = await courseRepository.Get(courseId);

        //    if (updatedCourse != null)
        //    {
        //        updatedCourse.StatusId = statusId;

        //        var result = await courseRepository.Update(updatedCourse);

        //        if (result != null)
        //        {
        //            return Ok(new
        //            {

        //                Results = result
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new
        //            {
        //                Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
        //            });
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(new
        //        {
        //            Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
        //        });
        //    }
        //}



        //[HttpGet]
        //[Route("GetAllByLecturerId")]
        //public async Task<IActionResult> GetAllByLecturerId([FromQuery] string lecturerId)
        //{
        //    var courses = await courseRepository.GetAllByLecturerId(lecturerId);
        //    return Ok(new
        //    {
        //        Results = courses
        //    });
        //}


        //[HttpGet()]
        //[Route("BestSellerCoursesByCategoryTypeId")]
        //public async Task<IActionResult> BestSellerCoursesByCategoryTypeId([FromQuery] int categoryTypeId, [FromQuery] int pageSize, [FromQuery] int pageOffset)
        //{
        //    var bestSellerCourses = await courseRepository.GetBestSellerCoursesByCategoryTypeId(categoryTypeId, pageSize, pageOffset);

        //    return Ok(new
        //    {
        //        Results = bestSellerCourses
        //    });
        //}


        //[HttpGet()]
        //[Route("OutstandingCoursesByCategoryId")]
        //public async Task<IActionResult> OutstandingCoursesByCategoryId([FromQuery] int categoryId, [FromQuery] int pageSize, [FromQuery] int pageOffset)
        //{
        //    var outstandingCourses = await courseRepository.GetOutStandingCoursesByCategoryId(categoryId, pageSize, pageOffset);

        //    return Ok(new
        //    {
        //        Results = outstandingCourses
        //    });
        //}


        ////For testing
        //[HttpPost]
        //[Route("CreateRange")]
        //public async Task<IActionResult> CreateRange([FromBody] IList<CourseViewModel> courseViewModel)
        //{
        //    IList<Course> courses = new List<Course>();

        //    foreach(var c in courseViewModel)
        //    {
        //        Course course = new Course()
        //        {
        //            Name = c.Name,
        //            CategoryId = c.CategoryId,
        //            LecturerId = c.LecturerId,
        //            ImageUrl = c.ImageUrl,
        //            Price = c.Price,
        //            Discount = c.Discount,
        //            ShortDiscription = c.ShortDiscription,
        //            DetailDiscription = c.DetailDiscription,
        //            LastUpdated = DateTime.Now,
        //            StatusId = c.StatusId
        //        };

        //        courses.Add(course);
        //    }


        //    var result = await courseRepository.AddRange(courses);

        //    if (result != null)
        //    {
        //        return Ok(new
        //        {
        //            Results = result
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new
        //        {
        //            Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" }
        //        });
        //    }
        //}
    }
}
