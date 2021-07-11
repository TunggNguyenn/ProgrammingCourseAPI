using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class CartViewModel
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public List<int> CourseIds { get; set; }
    }
}
