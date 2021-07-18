using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class ResendOTPViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
