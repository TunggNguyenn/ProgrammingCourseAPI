using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class StudentCourseViewModel
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
