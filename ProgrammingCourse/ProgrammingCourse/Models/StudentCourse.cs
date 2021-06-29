using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class StudentCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]  
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public User Student { get; set; }

        [Required] 
        public int CourseId { get; set; } 

        [ForeignKey("CourseId")]
        public Course Course { get; set; }


        [Required]
        public DateTime DateTime { get; set; }

    }
}
