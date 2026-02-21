using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.User
{
    public class RejectRequestDto
    {
        public int UserId { get; set; }
        public string RejectReason { get; set; }
    }
}
