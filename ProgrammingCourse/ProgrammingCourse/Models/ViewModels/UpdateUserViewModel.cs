using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        public string UserId { get; set; }
        public string NewUserName { get; set; }
        public string NewAvatarUrl { get; set; }
        public string NewEmail { get; set; }
    }
}
