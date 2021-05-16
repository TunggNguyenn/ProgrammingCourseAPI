using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        public int? CategoryTypeId { get; set; }

        [ForeignKey("CategoryTypeId")]
        public CategoryType CategoryType { get; set; }

        public IList<Course> Courses { get; set; }

    }
}
