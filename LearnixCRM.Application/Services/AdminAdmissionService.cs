using LearnixCRM.Application.DTOs.Student;
using LearnixCRM.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class AdminAdmissionService
    {
        private readonly IAdminAdmissionRepository _adminAdmissionRepository;
        public AdminAdmissionService(IAdminAdmissionRepository adminAdmissionRepository)
        {
            _adminAdmissionRepository = adminAdmissionRepository;
        }
        public async Task<IEnumerable<AdminStudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _adminAdmissionRepository.GetAllAsync();

            if (students == null || !students.Any())
                throw new KeyNotFoundException("No students found.");

            return students;

          
        }

    }
}
