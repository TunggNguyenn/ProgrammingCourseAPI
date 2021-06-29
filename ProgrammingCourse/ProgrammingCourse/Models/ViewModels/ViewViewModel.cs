using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class ViewViewModel
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int Number { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]  //
        public string CourseId { get; set; }
    }
}
