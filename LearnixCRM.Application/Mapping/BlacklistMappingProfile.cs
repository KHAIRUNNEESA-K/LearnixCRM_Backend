using AutoMapper;
using LearnixCRM.Application.DTOs.Blacklist;
using LearnixCRM.Domain.Entities;

namespace LearnixCRM.Application.MappingProfiles
{
    public class BlacklistMappingProfile : Profile
    {
        public BlacklistMappingProfile()
        {
            CreateMap<Blacklist, BlacklistResponseDto>();
        }
    }
}