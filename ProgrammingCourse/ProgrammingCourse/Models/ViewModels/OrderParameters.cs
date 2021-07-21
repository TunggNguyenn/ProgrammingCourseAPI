using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class OrderParameters
    {
        public bool RatingDescending { get; set; } = false;

        public bool PriceIncreasing { get; set; } = false;
    }
}
