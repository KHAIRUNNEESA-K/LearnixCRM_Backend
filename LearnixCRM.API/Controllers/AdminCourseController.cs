using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Course;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/courses")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _service;

        public AdminCourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _service.GetAllAsync();

            return Ok(ApiResponse<IEnumerable<CourseResponseDto>>.SuccessResponse(
                courses,
                "Courses fetched successfully"
            ));
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var course = await _service.GetByIdAsync(courseId);

            return Ok(ApiResponse<CourseResponseDto>.SuccessResponse(
                course,
                "Course fetched successfully"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
        {
            int adminUserId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var course = await _service.CreateAsync(dto, adminUserId);

            return StatusCode(201, ApiResponse<CourseResponseDto>.SuccessResponse(
                course,
                "Course created successfully"
            ));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseDto dto)
        {
            int adminUserId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var updatedCourse = await _service.UpdateAsync(dto, adminUserId);

            return Ok(ApiResponse<CourseResponseDto>.SuccessResponse(
                updatedCourse,
                "Course updated successfully"
            ));
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            int adminUserId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _service.DeleteAsync(courseId, adminUserId);

            return Ok(ApiResponse<string>.SuccessResponse(
                null,
                "Course deleted successfully"
            ));
        }
    }
}