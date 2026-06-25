using AutoMapper;
using PayrollFeature.Domain;

namespace PayrollFeature.Application.DTO
{
    public class PayrollMapper : Profile
    {
        public PayrollMapper()
        {
            CreateMap<PayrollDto, PayrollRecord>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PayrollId))
                .ReverseMap()
                .ForMember(dest => dest.PayrollId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
