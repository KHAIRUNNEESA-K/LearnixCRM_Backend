using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Configuration
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; } = default!;

        public string FromEmail { get; set; } = default!;

        public string FromName { get; set; } = default!;
    }
}
