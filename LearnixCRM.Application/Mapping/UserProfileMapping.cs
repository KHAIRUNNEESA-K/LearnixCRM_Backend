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
    public  class UserProfileMapping :Profile
    {
        public UserProfileMapping()
        {
            CreateMap<User, UserProfileDto>();

            CreateMap<UpdateProfileDto, User>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
