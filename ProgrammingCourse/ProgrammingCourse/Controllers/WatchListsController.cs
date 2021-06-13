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
    public class WatchListsController : ControllerBase
    {
        private WatchListRepository watchListRepository;

        public WatchListsController(WatchListRepository watchListRepo)
        {
            watchListRepository = watchListRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var watchList = await watchListRepository.Get(id);

            if (watchList != null)
            {
                return Ok(new
                {
                    Results = watchList
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
            var watchLists = await watchListRepository.GetAll();
            return Ok(new
            {
                Results = watchLists
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] WatchListViewModel watchListViewModel)
        {
            bool isExistedWatchListByStudentIdAndCourseId = await watchListRepository.IsExistedWatchListByStudentIdAndCourseId(watchListViewModel.StudentId, watchListViewModel.CourseId);

            if (isExistedWatchListByStudentIdAndCourseId == true)
            {
                return BadRequest(new
                {
                    Errors = new { Code = "ExistedWatchList", Description = "WatchList has already existed!" } 
                });
            }

            WatchList watchList = new WatchList() { StudentId = watchListViewModel.StudentId, CourseId = watchListViewModel.CourseId };

            var result = await watchListRepository.Add(watchList);

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
        public async Task<IActionResult> Update([FromForm] WatchListViewModel watchListViewModel)
        {
            var updatedWatchList = await watchListRepository.Get(watchListViewModel.Id);

            if (updatedWatchList != null)
            {
                updatedWatchList.StudentId = watchListViewModel.StudentId;
                updatedWatchList.CourseId = watchListViewModel.CourseId;

                var result = await watchListRepository.Update(updatedWatchList);

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
            var deletedWatchList = await watchListRepository.Delete(id);

            if (deletedWatchList != null)
            {
                return Ok(new
                {
                    Results = deletedWatchList
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
        [Route("GetAllByStudentId")]
        public async Task<IActionResult> GetAllByStudentId([FromQuery] string studentId)
        {
            var watchLists = await watchListRepository.GetAllByStudentId(studentId);
            return Ok(new
            {
                Results = watchLists
            });
        }


        [HttpGet]
        [Route("IsExistedWatchListByStudentIdAndCourseId")]
        public async Task<IActionResult> IsExistedWatchListByStudentIdAndCourseId([FromQuery] string studentId, [FromQuery] int courseId)
        {
            bool isExistedWatchListByStudentIdAndCourseId = await watchListRepository.IsExistedWatchListByStudentIdAndCourseId(studentId, courseId);
            return Ok(new
            {
                Results = isExistedWatchListByStudentIdAndCourseId
            });
        }
    }
}
