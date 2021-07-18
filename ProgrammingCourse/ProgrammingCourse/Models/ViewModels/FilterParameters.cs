using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models.ViewModels
{
    public class FilterParameters
    {
        public string Search { get; set; } = "";

        public int CategoryId { get; set; } = 0;

        public int CategoryTypeId { get; set; } = 0;

        public int MinPrice { get; set; } = 0;

        public int MaxPrice { get; set; } = 50000000;

        public float MinRating { get; set; } = 0;

        public float MaxRating { get; set; } = 5;
    }
}
