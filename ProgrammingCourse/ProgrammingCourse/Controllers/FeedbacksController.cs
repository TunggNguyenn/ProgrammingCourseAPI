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
    public class FeedbacksController : ControllerBase
    {
        private readonly FeedbackRepository feedbackRepository;
        private readonly IMapper mapper;

        public FeedbacksController(FeedbackRepository feedbackRepository, IMapper mapper)
        {
            this.feedbackRepository = feedbackRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await feedbackRepository.GetById(id);

            if (feedback != null)
            {
                return Ok(new
                {
                    Results = feedback
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
            var feedbacks = await feedbackRepository.GetAll();
            return Ok(new
            {
                Results = feedbacks
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FeedbackViewModel feedbackViewModel)
        {
            try
            {
                bool isExistedFeedbackByUserIdAndCourseId = await feedbackRepository.IsExistedFeedbackByStudentIdAndCourseId(feedbackViewModel.UserId, feedbackViewModel.CourseId);

                if (isExistedFeedbackByUserIdAndCourseId == true)
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "ExistedFeedback", Description = "Feedback has already existed!" }
                    });
                }

                Feedback feedbackMapped = mapper.Map<Feedback>(feedbackViewModel);

                await feedbackRepository.Add(feedbackMapped);

                return Ok(new
                {
                    Results = feedbackMapped
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
        public async Task<IActionResult> Update([FromBody] FeedbackViewModel feedbackViewModel)
        {
            try
            {
                Feedback feedbackMapped = mapper.Map<Feedback>(feedbackViewModel);

                await feedbackRepository.Update(feedbackMapped);

                return Ok(new
                {
                    Results = feedbackMapped
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
                var removedFeedback = await feedbackRepository.GetById(id);

                await feedbackRepository.Remove(removedFeedback);

                return Ok(new
                {
                    Results = removedFeedback
                });
            }
            catch(Exception e)
            {
                Console.WriteLine($"ErrorMesages: {e}");

                return BadRequest(new
                {
                    Errors = new { Code = "InvalidId", Description = "Invalid Id!" }
                });
            }
        }


        [HttpGet]
        [Route("IsExistedFeedbackByStudentIdAndCourseId")]
        public async Task<IActionResult> IsExistedFeedbackByStudentIdAndCourseId([FromQuery] string studentId, [FromQuery] int courseId)
        {
            bool isExistedFeedbackByUserIdAndCourseId = await feedbackRepository.IsExistedFeedbackByStudentIdAndCourseId(studentId, courseId);

            return Ok(new
            {
                Results = isExistedFeedbackByUserIdAndCourseId
            });
        }


        [HttpGet]
        [Route("GetFeedbackListByCourseId")]
        public async Task<IActionResult> GetFeedbackListByCourseId([FromQuery] int courseId)
        {
            var feedbacks = await feedbackRepository.GetFeedbackListByCourseId(courseId);
            return Ok(new
            {
                Results = feedbacks
            });
        }
    }
}
