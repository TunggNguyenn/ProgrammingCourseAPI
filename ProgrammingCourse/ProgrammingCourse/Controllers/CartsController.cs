using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using ProgrammingCourse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly CartRepository cartRepository;
        private readonly CourseService courseService;
        private readonly IMapper mapper;

        public CartsController(CartRepository cartRepository, CourseService courseService, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Cart> carts = await cartRepository.GetAll();

            return Ok(new
            {
                Results = carts
            });
        }


        [HttpGet]
        [Route("GetCourseListByStudentId")]
        public async Task<IActionResult> GetCourseListByStudentId([FromQuery] string studentId)
        {
            try
            {
                List<int> courseIdList = await cartRepository.GetCourseIdListByStudentId(studentId);

                if(courseIdList.Count == 0)
                {
                    return Ok(new
                    {
                        Results = new object[] { }
                    });
                }
                else
                {
                    List<dynamic> dynamicCourses = new List<dynamic>();

                    foreach(int courseId in courseIdList)
                    {
                        var dynamicCourse = await courseService.GetWithAllInfoById(courseId);
                        dynamicCourses.Add(dynamicCourse);
                    }

                    return Ok(new
                    {
                        Results = dynamicCourses
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
        [Route("AddCourseToCart")]
        public async Task<IActionResult> AddCourseToCart([FromBody] CartViewModel cartViewModel)
        {
            try
            {
                Cart cart = await cartRepository.GetByCourseIdAndStudentId(cartViewModel.CourseId, cartViewModel.StudentId);

                if (cart == null)
                {
                    Cart cartMapped = mapper.Map<Cart>(cartViewModel);
                    await cartRepository.Add(cartMapped);

                    return Ok(new
                    {
                        Results = cartMapped
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "CourseExistedInCart", Description = "Course has already existed in Cart!" }
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
        [Route("RemoveCourseFromCart")]
        public async Task<IActionResult> RemoveCourseFromCart([FromBody] CartViewModel cartViewModel)
        {
            try
            {
                Cart cart = await cartRepository.GetByCourseIdAndStudentId(cartViewModel.CourseId, cartViewModel.StudentId);

                if (cart != null)
                {
                    await cartRepository.Remove(cart);

                    return Ok(new
                    {
                        Results = cart
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
            catch (Exception e)
            {
                Console.WriteLine($"ErrorMesages: {e}");

                return BadRequest(new
                {
                    Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" }
                });
            }
        }



        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] RemoveCartViewModel removeCartViewModel)
        {
            try
            {
                List<Cart> cartList = await cartRepository.GetCartListByStudentId(removeCartViewModel.StudentId);

                if(cartList.Count != 0)
                {
                    await cartRepository.RemoveRange(cartList);
                }


                return Ok(new
                {
                    Results = cartList
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
