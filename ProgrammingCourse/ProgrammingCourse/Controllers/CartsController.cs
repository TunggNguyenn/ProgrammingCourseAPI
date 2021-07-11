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
    public class CartsController : ControllerBase
    {
        private readonly CartRepository cartRepository;
        private readonly CourseCartRepository courseCartRepository;

        public CartsController(CartRepository cartRepository, CourseCartRepository courseCartRepository)
        {
            this.cartRepository = cartRepository;
            this.courseCartRepository = courseCartRepository;
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
        [Route("GetByStudentId")]
        public async Task<IActionResult> GetByStudentId([FromQuery] string studentId)
        {
            try
            {
                Cart cart = await cartRepository.GetByStudentId(studentId);

                return Ok(new
                {
                    Results = cart
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

        [HttpGet]
        [Route("GetWithCourseCartListByStudentId")]
        public async Task<IActionResult> GetWithCourseCartListByStudentId([FromQuery] string studentId)
        {
            try
            {
                Cart cart = await cartRepository.GetWithCourseCartListByStudentId(studentId);

                return Ok(new
                {
                    Results = cart
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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartViewModel cartViewModel)
        {
            try
            {
                Cart cart = await cartRepository.GetByStudentId(cartViewModel.StudentId);

                if (cart == null)
                {
                    cart = new Cart()
                    {
                        StudentId = cartViewModel.StudentId,
                        LastUpdated = DateTime.Now
                    };

                    await cartRepository.Add(cart);
                }
                else
                {
                    cart.LastUpdated = DateTime.Now;
                    await cartRepository.Update(cart);
                }

                List<CourseCart> newCourseCarts = new List<CourseCart>();

                foreach(int courseId in cartViewModel.CourseIds)
                {
                    CourseCart newCourseCart = new CourseCart()
                    {
                        CourseId = courseId,
                        CartId = cart.Id
                    };

                    newCourseCarts.Add(newCourseCart);
                }


                await courseCartRepository.AddRange(newCourseCarts);

                return Ok(new
                {
                    Results = cart
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



        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] CartViewModel cartViewModel)
        {
            try
            {
                Cart cart = await cartRepository.GetByStudentId(cartViewModel.StudentId);

                cart.LastUpdated = DateTime.Now;
                await cartRepository.Update(cart);



                var removedCourseCarts = await courseCartRepository.RemoveByCourseIdsAndCartId(cartViewModel.CourseIds, cart.Id);

                if (removedCourseCarts != null)
                {
                    cart.CourseCarts = removedCourseCarts;
                }

                return Ok(new
                {
                    Results = new
                    {
                        Cart = cart
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
