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
    public class StudentCoursesController : ControllerBase
    {
        private StudentCourseRepository studentCourseRepository;

        public StudentCoursesController(StudentCourseRepository studentCourseRepo)
        {
            studentCourseRepository = studentCourseRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var studentCourse = await studentCourseRepository.Get(id);

            if (studentCourse != null)
            {
                return Ok(new
                {
                    Results = studentCourse,
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
            var studentCourses = await studentCourseRepository.GetAll();
            return Ok(new
            {
                Results = studentCourses,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentCourseViewModel studentCourseViewModel)
        {
            StudentCourse studentCourse = new StudentCourse() { StudentId = studentCourseViewModel.StudentId, CourseId = studentCourseViewModel.CourseId };

            var result = await studentCourseRepository.Add(studentCourse);

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
        public async Task<IActionResult> Update([FromForm] StudentCourseViewModel studentCourseViewModel)
        {
            var updatedStudentCourse = await studentCourseRepository.Get(studentCourseViewModel.Id);

            if (updatedStudentCourse != null)
            {
                updatedStudentCourse.StudentId = studentCourseViewModel.StudentId;
                updatedStudentCourse.CourseId = studentCourseViewModel.CourseId;

                var result = await studentCourseRepository.Update(updatedStudentCourse);

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
            var deletedStudentCourse = await studentCourseRepository.Delete(id);

            if (deletedStudentCourse != null)
            {
                return Ok(new
                {
                    Results = deletedStudentCourse
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
    }
}
