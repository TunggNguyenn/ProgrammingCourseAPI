using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }

        [Required]
        public int Rate { get; set; }

        public string Review { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
