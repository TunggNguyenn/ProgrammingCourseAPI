using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class WatchListViewModel
    {
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
