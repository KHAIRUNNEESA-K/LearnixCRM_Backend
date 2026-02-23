using AutoMapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Mapping
{
    public class RegisterUserMappingProfile : Profile
    {
        public RegisterUserMappingProfile()
        {
            CreateMap<User, RegisterUserResponseDto>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => (int)src.UserRole))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.ContactNumber,
                    opt => opt.MapFrom(src => src.ContactNumber));

        }
    }
}
