using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Course
{
    public class CreateCourseDto
    {
        public string Name { get; set; } = null!;
        public int CourseDuration { get; set; }
        public decimal Fee { get; set; }
    }
}
