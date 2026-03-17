using AutoMapper;
using LearnixCRM.Application.DTOs.Course;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;

namespace LearnixCRM.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto, int userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var exists = await _repository.ExistsByNameAsync(dto.Name);

            if (exists)
                throw new InvalidOperationException("Course already exists");

            var course = Course.Create(
                dto.Name,
                dto.Fee,
                dto.CourseDuration,
                userId);

            await _repository.AddAsync(course);

            return _mapper.Map<CourseResponseDto>(course);
        }

        public async Task<List<CourseResponseDto>> GetAllAsync()
        {
            var courses = await _repository.GetAllAsync();

            return _mapper.Map<List<CourseResponseDto>>(courses);
        }

        public async Task<CourseResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid course id");

            var course = await _repository.GetByIdAsync(id);

            if (course == null)
                throw new KeyNotFoundException("Course not found");

            return _mapper.Map<CourseResponseDto>(course);
        }

        public async Task<CourseResponseDto> UpdateAsync(UpdateCourseDto dto, int userId)
        {
            if (dto.courseId <= 0)
                throw new ArgumentException("Invalid course id");

            var course = await _repository.GetByIdAsync(dto.courseId);

            if (course == null)
                throw new KeyNotFoundException("Course not found");

            if (!course.IsActive)
                throw new InvalidOperationException("Cannot update inactive course");

            course.Update(dto.Name,dto.Fee,dto.CourseDuration, userId);

            await _repository.UpdateAsync(course);

            return _mapper.Map<CourseResponseDto>(course);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid course id");

            var course = await _repository.GetByIdAsync(id);

            if (course == null)
                throw new KeyNotFoundException("Course not found");

            if (!course.IsActive)
                throw new InvalidOperationException("Course already deleted");

            course.Delete(userId);

            await _repository.DeleteAsync(course);
        }
    }
}