using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string AvatarUrl { get; set; }

        public virtual IList<WatchList> WatchLists { get; set; }

        public virtual IList<Course> Courses { get; set; }

        public virtual IList<StudentCourse> StudentCourses { get; set; }

        public virtual IList<Feedback> Feedbacks { get; set; }

        public virtual IList<RefreshToken> RefreshTokens { get; set; }
    }
}
