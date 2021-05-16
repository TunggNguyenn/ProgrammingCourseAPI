using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Rate { get; set; }

        public string Review { get; set; }

        //[Required]
        public int? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        //[Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
