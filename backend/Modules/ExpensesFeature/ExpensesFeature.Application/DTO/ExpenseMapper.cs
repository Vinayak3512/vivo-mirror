using AutoMapper;
using ExpensesFeature.Domain;

namespace ExpensesFeature.Application.DTO
{
    public class ExpenseMapper : Profile
    {
        public ExpenseMapper()
        {
            CreateMap<ExpenseDto, ExpenseRecord>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExpenseId))
                .ReverseMap()
                .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
