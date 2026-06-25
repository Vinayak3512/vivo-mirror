using AutoMapper;
using AttendanceFeature.Domain;

namespace AttendanceFeature.Application.DTO
{
    public class CreateAttendanceMapper : Profile
    {
        public CreateAttendanceMapper()
        {
            CreateMap<CreateAttendanceDto, AttendanceRecord>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => nameof(AttendanceRecord)))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }

    public class UpdateAttendanceMapper : Profile
    {
        public UpdateAttendanceMapper()
        {
            CreateMap<UpdateAttendanceDto, AttendanceRecord>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AttendanceId))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => nameof(AttendanceRecord)))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }

    public class GetAllAttendanceMapper : Profile
    {
        public GetAllAttendanceMapper()
        {
            CreateMap<AttendanceRecord, GetAllAttendanceItem>();
        }
    }
}
