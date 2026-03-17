using LearnixCRM.Application.DTOs.Course;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LearnixCRM.Domain.Entities;

namespace LearnixCRM.Application.Mapping
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            // Entity → Response DTO
            CreateMap<Course, CourseResponseDto>();

            // Create DTO → Entity
            CreateMap<CreateCourseDto, Course>()
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.Leads, opt => opt.Ignore());

            // Update DTO → Entity
            CreateMap<UpdateCourseDto, Course>()
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.Leads, opt => opt.Ignore());
        }
    }
}
