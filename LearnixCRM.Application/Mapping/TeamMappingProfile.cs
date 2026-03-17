using AutoMapper;
using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Domain.Entities;

namespace LearnixCRM.Application.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamResponseDto>()
                .ForMember(dest => dest.ManagerName,
                    opt => opt.MapFrom(src => src.ManagerUser.FullName));

            CreateMap<Team, TeamWithMembersResponseDto>()
                .ForMember(dest => dest.ManagerName,
                    opt => opt.MapFrom(src => src.ManagerUser.FullName))
                .ForMember(dest => dest.SalesMembers,
                    opt => opt.MapFrom(src => src.Members));
        }
    }
}