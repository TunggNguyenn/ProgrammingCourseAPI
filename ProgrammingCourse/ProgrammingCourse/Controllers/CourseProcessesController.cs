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
    public class CourseProcessesController : ControllerBase
    {
        private readonly CourseProcessRepository courseProcessRepository;
        private readonly LectureRepository lectureRepository;

        public CourseProcessesController(CourseProcessRepository courseProcessRepository, LectureRepository lectureRepository)
        {
            this.courseProcessRepository = courseProcessRepository;
            this.lectureRepository = lectureRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<CourseProcess> courseProcesses = await courseProcessRepository.GetAll();

            return Ok(new
            {
                Results = courseProcesses
            });
        }


        [HttpGet]
        [Route("GetByStudentIdAndCourseId")]
        public async Task<IActionResult> GetByStudentIdAndCourseId([FromQuery] string studentId, int courseId)
        {
            try
            {
                CourseProcess courseProcess = await courseProcessRepository.GetByStudentIdAndCourseId(studentId, courseId);

                if (courseProcess == null)
                {

                    return Ok(new
                    {
                        Results = new
                        {
                            CourseProcess = courseProcess,
                            CompletionRate = 0.0
                        }
                    });
                }
                else
                {

                    //double completionRate = await lectureRepository.GetCompletionRateByCourseIdAndLectureId(courseProcess.CourseId, courseProcess.LectureId);


                    return Ok(new
                    {
                        Results = new
                        {
                            CourseProcess = courseProcess
                            //CompletionRate = completionRate
                        }
                    });
                }

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


        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CourseProcessViewModel courseProcessViewModel)
        {
            try
            {
                CourseProcess courseProcess = await courseProcessRepository.GetByStudentIdAndCourseId(courseProcessViewModel.StudentId, courseProcessViewModel.CourseId);

                if (courseProcess == null)
                {
                    courseProcess = new CourseProcess()
                    {
                        StudentId = courseProcessViewModel.StudentId,
                        CourseId = courseProcessViewModel.CourseId,
                        LectureId = courseProcessViewModel.LectureId,
                        LastUpdated = DateTime.Now
                    };

                    await courseProcessRepository.Add(courseProcess);
                }
                else
                {
                    courseProcess.LectureId = courseProcessViewModel.LectureId;
                    courseProcess.LastUpdated = DateTime.Now;

                    await courseProcessRepository.Update(courseProcess);
                }


                double completionRate = await lectureRepository.GetCompletionRateByCourseIdAndLectureId(courseProcessViewModel.CourseId, courseProcessViewModel.LectureId);


                return Ok(new
                {
                    Results = new
                    {
                        CourseProcess = courseProcess,
                        CompletionRate = completionRate
                    }
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
