using AutoMapper;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Domain.Entities;

namespace LearnixCRM.Application.Mappings
{
    public class LeadMappingProfile : Profile
    {
        public LeadMappingProfile()
        {
            CreateMap<Lead, LeadResponseDto>().ReverseMap();
            CreateMap<CreateLeadRequestDto, Lead>();
            CreateMap<UpdateLeadRequestDto, Lead>();
        }
    }
}