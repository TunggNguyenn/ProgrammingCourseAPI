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
        //private RefreshTokenRepository refreshTokenRepository;

        //public RefreshTokensController(RefreshTokenRepository refreshTokenRepo)
        //{
        //    refreshTokenRepository = refreshTokenRepo;
        //}


        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    var refreshToken = refreshTokenRepository.Get(id);

        //    if (refreshToken != null)
        //    {
        //        return Ok(new
        //        {
        //            Results = refreshToken
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new
        //        {
        //            Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
        //        });
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var refreshToken = refreshTokenRepository.GetAll();
        //    return Ok(new
        //    {
        //        Results = refreshToken
        //    });
        //}

        //[HttpDelete]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var result = await refreshTokenRepository.Remove(id);

        //    if (result != null)
        //    {
        //        return Ok(new
        //        {
        //            Results = result
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new
        //        {
        //            Errors = new { Code = "InvalidId", Description = "Invalid Id!" } 
        //        });
        //    }
        //}
    }
}
