using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Student;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/admission")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminAdmissionController :ControllerBase
    {
        private readonly IAdminAdmissionService _adminAdmissionService;
        public AdminAdmissionController(IAdminAdmissionService adminAdmissionService)
        {
            _adminAdmissionService = adminAdmissionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAdmissions()
        {
            var students = await _adminAdmissionService.GetAllStudentsAsync();

            return Ok(ApiResponse<IEnumerable<AdminStudentResponseDto>>
                .SuccessResponse(students, "All admissions retrieved successfully"));
        }
    }
}
