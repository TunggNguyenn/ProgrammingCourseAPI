using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class StatusViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
