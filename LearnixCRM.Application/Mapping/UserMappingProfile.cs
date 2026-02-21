using AutoMapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseDto>()
                    .ForMember(dest => dest.Role,
                         opt => opt.MapFrom(src => (int)src.UserRole))
                    .ForMember(dest => dest.Status,
                         opt => opt.MapFrom(src => (int)src.Status));

        }
    }
}
