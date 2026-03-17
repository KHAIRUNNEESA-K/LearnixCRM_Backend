using AutoMapper;
using LearnixCRM.Application.DTOs.Blacklist;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Mapping
{
    public class RejectResponseMappingProfile :Profile
    {
        public RejectResponseMappingProfile()
        {
            CreateMap<User, RejectResponseDto>();
        }
    }
}
