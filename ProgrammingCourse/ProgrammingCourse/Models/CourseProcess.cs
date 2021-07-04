using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingCourse.Models
{
    public class CourseProcess
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public User Student { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Required]
        public int LectureId { get; set; }

        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; }

        [Required]
        public int Time { get; set; }
    }
}
