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
    public class LecturesController : ControllerBase
    {
        private LectureRepository lectureRepository;

        public LecturesController(LectureRepository lectureRepo)
        {
            lectureRepository = lectureRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lecture = await lectureRepository.Get(id);

            if (lecture != null)
            {
                return Ok(new
                {
                    Results = lecture
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
            var lectures = await lectureRepository.GetAll();
            return Ok(new
            {
                Results = lectures
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] LectureViewModel lectureViewModel)
        {
            Lecture lecture = new Lecture() { Section = lectureViewModel.Section, Name = lectureViewModel.Name, VideoUrl = lectureViewModel.VideoUrl, CourseId = lectureViewModel.CourseId };

            var result = await lectureRepository.Add(lecture);

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
        public async Task<IActionResult> Update([FromForm] LectureViewModel lectureViewModel)
        {
            var updatedLecture = await lectureRepository.Get(lectureViewModel.Id);

            if (updatedLecture != null)
            {
                updatedLecture.Section = lectureViewModel.Section;
                updatedLecture.Name = lectureViewModel.Name;
                updatedLecture.VideoUrl = lectureViewModel.VideoUrl;
                updatedLecture.CourseId = lectureViewModel.CourseId;

                var result = await lectureRepository.Update(updatedLecture);

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
            var deletedLecture = await lectureRepository.Delete(id);

            if (deletedLecture != null)
            {
                return Ok(new
                {
                    Results = deletedLecture
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
        [Route("GetAllByCourseId")]
        public async Task<IActionResult> GetAllByCourseId([FromQuery] int courseId)
        {
            var lectures = await lectureRepository.GetAllByCourseId(courseId);
            return Ok(new
            {
                Results = lectures
            });
        }
    }
}
