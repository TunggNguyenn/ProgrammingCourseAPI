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
    public class StudentCoursesController : ControllerBase
    {
        //private StudentCourseRepository studentCourseRepository;

        //public StudentCoursesController(StudentCourseRepository studentCourseRepo)
        //{
        //    studentCourseRepository = studentCourseRepo;
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var studentCourse = await studentCourseRepository.Get(id);

        //    if (studentCourse != null)
        //    {
        //        return Ok(new
        //        {
        //            Results = studentCourse
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
        //public async Task<IActionResult> GetAll()
        //{
        //    var studentCourses = await studentCourseRepository.GetAll();
        //    return Ok(new
        //    {
        //        Results = studentCourses
        //    });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromForm] StudentCourseViewModel studentCourseViewModel)
        //{
        //    bool isParticipatedByStudentIdAndCourseId = await studentCourseRepository.IsParticipatedByStudentIdAndCourseId(studentCourseViewModel.StudentId, studentCourseViewModel.CourseId);
            
        //    if (isParticipatedByStudentIdAndCourseId == true)
        //    {
        //        return BadRequest(new
        //        {
        //            Errors = new { Code = "ParticipatedCourse", Description = "The student has already participated the course!" } 
        //        });
        //    }

        //    StudentCourse studentCourse = new StudentCourse()
        //    {
        //        StudentId = studentCourseViewModel.StudentId,
        //        CourseId = studentCourseViewModel.CourseId,
        //        DateTime = DateTime.Now
        //    };

        //    var result = await studentCourseRepository.Add(studentCourse);

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
        //            Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
        //        });
        //    }
        //}


        //[HttpPut]
        //public async Task<IActionResult> Update([FromForm] StudentCourseViewModel studentCourseViewModel)
        //{
        //    var updatedStudentCourse = await studentCourseRepository.Get(studentCourseViewModel.Id);

        //    if (updatedStudentCourse != null)
        //    {
        //        updatedStudentCourse.StudentId = studentCourseViewModel.StudentId;
        //        updatedStudentCourse.CourseId = studentCourseViewModel.CourseId;

        //        var result = await studentCourseRepository.Update(updatedStudentCourse);

        //        if (result != null)
        //        {
        //            return Ok(new
        //            {
        //                Results = result
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new
        //            {
        //                Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
        //            });
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(new
        //        {
        //            Errors = new { Code = "InvalidInputParameters", Description = "Invalid Input Parameters!" } 
        //        });
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var deletedStudentCourse = await studentCourseRepository.Delete(id);

        //    if (deletedStudentCourse != null)
        //    {
        //        return Ok(new
        //        {
        //            Results = deletedStudentCourse
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
        //[Route("GetAllByStudentId")]
        //public async Task<IActionResult> GetAllByStudentId([FromQuery] string studentId)
        //{
        //    var studentCourses = await studentCourseRepository.GetAllByStudentId(studentId);
        //    return Ok(new
        //    {
        //        Results = studentCourses
        //    });
        //}


        //[HttpGet]
        //[Route("GetAllByCourseId")]
        //public async Task<IActionResult> GetAllByCourseId([FromQuery] int courseId)
        //{
        //    var studentCourses = await studentCourseRepository.GetAllByCourseId(courseId);
        //    return Ok(new
        //    {
        //        Results = studentCourses
        //    });
        //}


        //[HttpGet]
        //[Route("IsParticipatedByStudentIdAndCourseId")]
        //public async Task<IActionResult> IsParticipatedByStudentIdAndCourseId([FromQuery] string studentId, [FromQuery] int courseId)
        //{
        //    bool isParticipatedByStudentIdAndCourseId = await studentCourseRepository.IsParticipatedByStudentIdAndCourseId(studentId, courseId);
        //    return Ok(new
        //    {
        //        Results = isParticipatedByStudentIdAndCourseId
        //    });
        //}
    }
}
