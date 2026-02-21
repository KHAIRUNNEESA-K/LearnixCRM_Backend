using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.User
{
    public class UpdateProfileDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ContactNumber { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
