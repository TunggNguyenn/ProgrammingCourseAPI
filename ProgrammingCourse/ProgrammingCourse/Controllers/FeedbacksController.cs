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
        private FeedbackRepository feedbackRepository;

        public FeedbacksController(FeedbackRepository feedbackRepo)
        {
            feedbackRepository = feedbackRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var feedback = await feedbackRepository.Get(id);

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
                    Errors = new object[] { new { Code = "InvalidId", Description = "Invalid Id!" } }
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
        public async Task<IActionResult> Create([FromForm] FeedbackViewModel feedbackViewModel)
        {
            bool isExistedFeedbackByUserIdAndCourseId = await feedbackRepository.IsExistedFeedbackByStudentIdAndCourseId(feedbackViewModel.UserId, feedbackViewModel.CourseId);

            if(isExistedFeedbackByUserIdAndCourseId == true)
            {
                return BadRequest(new
                {
                    Errors = new object[] { new { Code = "ExistedFeedback", Description = "Feedback has already existed!" } }
                });
            }

            Feedback feedback = new Feedback() { Rate = feedbackViewModel.Rate, Review = feedbackViewModel.Review, CourseId = feedbackViewModel.CourseId, UserId = feedbackViewModel.UserId };

            var result = await feedbackRepository.Add(feedback);

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
        public async Task<IActionResult> Update([FromForm] FeedbackViewModel feedbackViewModel)
        {
            var updatedFeedback = await feedbackRepository.Get(feedbackViewModel.Id);

            if (updatedFeedback != null)
            {
                updatedFeedback.Rate = feedbackViewModel.Rate;
                updatedFeedback.Review = feedbackViewModel.Review;
                updatedFeedback.CourseId = updatedFeedback.CourseId;
                updatedFeedback.UserId = feedbackViewModel.UserId;

                var result = await feedbackRepository.Update(updatedFeedback);

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
            var deletedFeedback = await feedbackRepository.Delete(id);

            if (deletedFeedback != null)
            {
                return Ok(new
                {
                    Results = deletedFeedback
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
        [Route("GetAllByCourseId")]
        public async Task<IActionResult> GetAllByCourseId([FromQuery] int courseId)
        {
            var feedbacks = await feedbackRepository.GetAllByCourseId(courseId);
            return Ok(new
            {
                Results = feedbacks
            });
        }
    }
}
