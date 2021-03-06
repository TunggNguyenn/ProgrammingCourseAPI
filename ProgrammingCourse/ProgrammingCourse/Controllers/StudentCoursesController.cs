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
    public class StudentCoursesController : ControllerBase
    {
        private readonly StudentCourseRepository studentCourseRepository;
        private readonly IMapper mapper;

        public StudentCoursesController(StudentCourseRepository studentCourseRepository, IMapper mapper)
        {
            this.studentCourseRepository = studentCourseRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var studentCourse = await studentCourseRepository.GetById(id);

            if (studentCourse != null)
            {
                return Ok(new
                {
                    Results = studentCourse
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
            var studentCourses = await studentCourseRepository.GetAll();
            return Ok(new
            {
                Results = studentCourses
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentCourseViewModel studentCourseViewModel)
        {
            try
            {
                bool isParticipatedByStudentIdAndCourseId = await studentCourseRepository.IsParticipatedByStudentIdAndCourseId(studentCourseViewModel.StudentId, studentCourseViewModel.CourseId);

                if (isParticipatedByStudentIdAndCourseId == true)
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "ParticipatedCourse", Description = "The student has already participated the course!" }
                    });
                }

                StudentCourse studentCourseMapped = mapper.Map<StudentCourse>(studentCourseViewModel);
                studentCourseMapped.DateTime = DateTime.Now;

                await studentCourseRepository.Add(studentCourseMapped);

                return Ok(new
                {
                    Results = studentCourseMapped
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
        public async Task<IActionResult> Update([FromBody] StudentCourseViewModel studentCourseViewModel)
        {
            try
            {
                StudentCourse studentCourseMapped = mapper.Map<StudentCourse>(studentCourseViewModel);
                studentCourseMapped.DateTime = DateTime.Now;

                await studentCourseRepository.Update(studentCourseMapped);

                return Ok(new
                {
                    Results = studentCourseMapped
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
                var removedStudentCourse = await studentCourseRepository.GetById(id);

                await studentCourseRepository.Remove(removedStudentCourse);

                return Ok(new
                {
                    Results = removedStudentCourse
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


        [HttpGet]
        [Route("GetStudentCourseListByStudentId")]
        public async Task<IActionResult> GetStudentCourseListByStudentId([FromQuery] string studentId)
        {
            var studentCourses = await studentCourseRepository.GetStudentCourseListByStudentId(studentId);
            return Ok(new
            {
                Results = studentCourses
            });
        }



        [HttpGet]
        [Route("GetStudentCourseListByCourseId")]
        public async Task<IActionResult> GetStudentCourseListByCourseId([FromQuery] int courseId)
        {
            var studentCourses = await studentCourseRepository.GetStudentCourseListByCourseId(courseId);
            return Ok(new
            {
                Results = studentCourses
            });
        }


        [HttpGet]
        [Route("IsParticipatedByStudentIdAndCourseId")]
        public async Task<IActionResult> IsParticipatedByStudentIdAndCourseId([FromQuery] string studentId, [FromQuery] int courseId)
        {
            bool isParticipatedByStudentIdAndCourseId = await studentCourseRepository.IsParticipatedByStudentIdAndCourseId(studentId, courseId);
            return Ok(new
            {
                Results = isParticipatedByStudentIdAndCourseId
            });
        }


        [HttpPost]
        [Route("AddWithMultipleCourses")]
        public async Task<IActionResult> AddWithMultipleCourses([FromBody] List<StudentCourseViewModel> studentCourseViewModels)
        {
            try
            {
                List<StudentCourse> studentCourses = new List<StudentCourse>();

                for (int i = 0; i < studentCourseViewModels.Count; i++)
                {
                    bool isParticipatedByStudentIdAndCourseId = await studentCourseRepository.IsParticipatedByStudentIdAndCourseId(studentCourseViewModels[i].StudentId, studentCourseViewModels[i].CourseId);

                    if (isParticipatedByStudentIdAndCourseId == true)
                    {
                        return BadRequest(new
                        {
                            Errors = new { Code = "ParticipatedCourse", Description = "The student has already participated the course!" }
                        });
                    }


                    StudentCourse studentCourseMapped = mapper.Map<StudentCourse>(studentCourseViewModels[i]);
                    studentCourseMapped.DateTime = DateTime.Now;

                    //await studentCourseRepository.Add(studentCourseMapped);

                    studentCourses.Add(studentCourseMapped);
                }

                await studentCourseRepository.AddRange(studentCourses);

                return Ok(new
                {
                    Results = studentCourses
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
    }
}
