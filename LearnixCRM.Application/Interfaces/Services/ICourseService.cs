using LearnixCRM.Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface ICourseService
    {
        Task<CourseResponseDto> CreateAsync(CreateCourseDto dto, int userId);
        Task<List<CourseResponseDto>> GetAllAsync();
        Task<CourseResponseDto> GetByIdAsync(int id);
        Task<CourseResponseDto> UpdateAsync(UpdateCourseDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
