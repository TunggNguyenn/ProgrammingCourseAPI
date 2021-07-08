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
    public class LecturesController : ControllerBase
    {
        private readonly LectureRepository lectureRepository;
        private readonly IMapper mapper;

        public LecturesController(LectureRepository lectureRepository, IMapper mapper)
        {
            this.lectureRepository = lectureRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lecture = await lectureRepository.GetById(id);

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
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" }
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
        public async Task<IActionResult> Add([FromBody] LectureViewModel lectureViewModel)
        {
            try
            {
                Lecture lectureMapped = mapper.Map<Lecture>(lectureViewModel);

                await lectureRepository.Add(lectureMapped);

                return Ok(new
                {
                    Results = lectureMapped
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
        public async Task<IActionResult> Update([FromBody] LectureViewModel lectureViewModel)
        {
            try
            {
                Lecture lectureMapped = mapper.Map<Lecture>(lectureViewModel);

                await lectureRepository.Update(lectureMapped);

                return Ok(new
                {
                    Results = lectureMapped
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
                var removedLecture = await lectureRepository.GetById(id);

                await lectureRepository.Remove(removedLecture);

                return Ok(new
                {
                    Results = removedLecture
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
        [Route("GetLectureListByCourseId")]
        public async Task<IActionResult> GetLectureListByCourseId([FromQuery] int courseId)
        {
            var lectures = await lectureRepository.GetLectureListByCourseId(courseId);
            return Ok(new
            {
                Results = lectures
            });
        }
    }
}
