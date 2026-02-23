using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.FollowUp
{
    public class CreateFollowUpRequestDto
    {
        public int LeadId { get; set; }

        public DateTime FollowUpDate { get; set; }

        public string Remark { get; set; } = string.Empty;
    }
}
