using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string UserId { get; set; }

        //[Required]
        public string NewUserName { get; set; }

        //[Required]
        public string NewAvatarUrl { get; set; }

        //[Required]
        public string NewEmail { get; set; }

        //[Required]
        public string Description { get; set; }
    }
}
