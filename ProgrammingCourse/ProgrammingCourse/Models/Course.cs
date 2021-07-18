using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]  
        public int CategoryId { get; set; } 

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        //[Required]  
        public string LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public User Lecturer { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        public float Discount { get; set; }

        [Required]
        public virtual IList<View> Views { get; set; }

        [Required]
        public string ShortDiscription { get; set; }

        [Required]
        public string DetailDiscription { get; set; }

        public DateTime LastUpdated { get; set; }

        [Required] 
        public int StatusId { get; set; } 

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        public virtual IList<Lecture> Lectures { get; set; }

        public virtual IList<Feedback> Feedbacks { get; set; }

        public virtual IList<StudentCourse> StudentCourses { get; set; }

        public virtual IList<WatchList> WatchLists { get; set; }
    }
}
