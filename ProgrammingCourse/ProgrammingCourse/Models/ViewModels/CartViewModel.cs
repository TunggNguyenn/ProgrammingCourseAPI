using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class CartViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        public virtual IList<CourseCart> CourseCarts { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
