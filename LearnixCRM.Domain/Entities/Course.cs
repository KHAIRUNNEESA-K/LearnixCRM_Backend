using LearnixCRM.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Entities
{
    public class Course:AuditableEntity
    {
        public int CourseId { get; private set; }

        public string Name { get; private set; } = null!;

        public decimal Fee { get; private set; }
        public bool IsActive { get; private set; }

        public ICollection<Lead> Leads { get; private set; } = new List<Lead>();


    }
}
