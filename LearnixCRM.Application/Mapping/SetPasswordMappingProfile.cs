using AutoMapper;
using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Mapping
{
    public class SetPasswordMappingProfile : Profile
    {
        public SetPasswordMappingProfile()
        {
            CreateMap<User, SetPasswordResponseDto>()
                 .ForMember(dest => dest.Role,
                 opt => opt.MapFrom(src => src.UserRole.ToString()));


        }
    }
}
