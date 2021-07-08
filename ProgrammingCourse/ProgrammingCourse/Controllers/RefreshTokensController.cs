using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokensController : ControllerBase
    {
        private readonly RefreshTokenRepository refreshTokenRepository;
        private readonly IMapper mapper;

        public RefreshTokensController(RefreshTokenRepository refreshTokenRepository, IMapper mapper)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var refreshToken = await refreshTokenRepository.GetById(id);

            if (refreshToken != null)
            {
                return Ok(new
                {
                    Results = refreshToken
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
            var refreshTokens = await refreshTokenRepository.GetAll();
            return Ok(new
            {
                Results = refreshTokens
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var result = await refreshTokenRepository.Remove(id);

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
                        Errors = new { Code = "InvalidId", Description = "Invalid Id!" }
                    });
                }
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
    }
}
