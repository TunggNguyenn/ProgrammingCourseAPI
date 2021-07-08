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
    public class WatchListsController : ControllerBase
    {
        private readonly WatchListRepository watchListRepository;
        private readonly IMapper mapper;

        public WatchListsController(WatchListRepository watchListRepository, IMapper mapper)
        {
            this.watchListRepository = watchListRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var watchList = await watchListRepository.GetById(id);

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
        public async Task<IActionResult> Add([FromBody] WatchListViewModel watchListViewModel)
        {
            try
            {
                bool isExistedWatchListByStudentIdAndCourseId = await watchListRepository.IsExistedWatchListByStudentIdAndCourseId(watchListViewModel.StudentId, watchListViewModel.CourseId);

                if (isExistedWatchListByStudentIdAndCourseId == true)
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "ExistedWatchList", Description = "WatchList has already existed!" }
                    });
                }

                WatchList watchListMapped = mapper.Map<WatchList>(watchListViewModel);

                await watchListRepository.Add(watchListMapped);

                return Ok(new
                {
                    Results = watchListMapped
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
        public async Task<IActionResult> Update([FromBody] WatchListViewModel watchListViewModel)
        {
            try
            {
                WatchList watchListMapped = mapper.Map<WatchList>(watchListViewModel);

                await watchListRepository.Update(watchListMapped);

                return Ok(new
                {

                    Results = watchListMapped
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
                var removedWatchList = await watchListRepository.GetById(id);

                await watchListRepository.Remove(removedWatchList);

                return Ok(new
                {
                    Results = removedWatchList
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
        [Route("GetWatchListListByStudentId")]
        public async Task<IActionResult> GetWatchListListByStudentId([FromQuery] string studentId)
        {
            var watchLists = await watchListRepository.GetWatchListListByStudentId(studentId);
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
