using AutoMapper;
using LearnixCRM.Application.DTOs.FollowUp;
using LearnixCRM.Domain.Entities;

public class FollowUpMappingProfile : Profile
{
    public FollowUpMappingProfile()
    {
        CreateMap<FollowUp, FollowUpResponseDto>()
            .ForMember(dest => dest.FollowUpId, opt => opt.MapFrom(src => src.FollowUpId))
            .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId))
            .ForMember(dest => dest.FollowUpDate, opt => opt.MapFrom(src => src.FollowUpDate))
            .ForMember(dest => dest.Remark, opt => opt.MapFrom(src => src.Remark));
    }
}