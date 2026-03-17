using LearnixCRM.Application.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IAdminAdmissionRepository
    {
        Task<IEnumerable<AdminStudentResponseDto>> GetAllAsync();
    }
}
