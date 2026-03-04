using LearnixCRM.Application.DTOs.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface ILeadImportService
    {
        Task<LeadImportResultDto> ImportLeadsAsync(Stream stream, int currentUserId);
    }
}
