using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class VerifyTwoStepVerificationViewModel
    {
        public string Email { get; set; }
        public int OTPCode { get; set; }
    }
}
