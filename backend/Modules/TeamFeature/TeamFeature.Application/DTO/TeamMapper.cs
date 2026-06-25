using AutoMapper;
using TeamFeature.Domain;

namespace TeamFeature.Application.DTO
{
    public class TeamMapper : Profile
    {
        public TeamMapper()
        {
            CreateMap<TeamMemberDto, TeamMember>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MemberId))
                .ReverseMap()
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
