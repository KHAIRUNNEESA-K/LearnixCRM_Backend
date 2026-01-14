using AutoMapper;
using LearnixCRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<LoginUserDto, LoginResponseDto>()
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.UserRole))
                .ForMember(dest => dest.Token,
                    opt => opt.Ignore()); 
        }
    }
}
