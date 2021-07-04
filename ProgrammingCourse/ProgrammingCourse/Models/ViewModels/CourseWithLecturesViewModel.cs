using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class CourseWithLecturesViewModel
    {
        [Required]
        public CourseViewModel CourseViewModel { get; set; }

        public virtual IList<LectureViewModel> LectureViewModels { get; set; }
    }
}
