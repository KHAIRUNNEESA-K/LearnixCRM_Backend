using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Student;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/sales/students")]
    [Authorize(Policy = "SalesOnly")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var students = await _studentService.GetAllStudentsAsync();

            return Ok(ApiResponse<IEnumerable<StudentResponseDto>>
                .SuccessResponse(students, "All students retrieved successfully"));
        }

        [HttpGet("{studentId:int}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var student = await _studentService.GetStudentByIdAsync(studentId);

            return Ok(ApiResponse<StudentResponseDto>
                .SuccessResponse(student, "Student retrieved successfully"));
        }
    }
}