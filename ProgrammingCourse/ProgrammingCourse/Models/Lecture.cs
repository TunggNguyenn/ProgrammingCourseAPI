using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Section { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        [Required]  
        public int CourseId { get; set; } 

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Required]
        public string Discription { get; set; }

        [Required]
        public int Duration { get; set; }
    }
}
