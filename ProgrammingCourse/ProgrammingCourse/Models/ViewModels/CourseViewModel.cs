using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class CourseViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int LecturerId { get; set; }


        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        public double DiscountPrice { get; set; }

        [Required]
        public int View { get; set; }

        [Required]
        public string ShortDiscription { get; set; }

        [Required]
        public string DetailDiscription { get; set; }

        public DateTime LastUpdated { get; set; }

        [Required]
        public int StatusId { get; set; }
    }
}
