using HRMS.Shared.Domain.Entity;

namespace AttendanceFeature.Application.DTO
{
    public class CreateAttendanceResponse
    {
        public string? AttendanceId { get; set; }
    }

    public class UpdateAttendanceResponse
    {
        public string? AttendanceId { get; set; }
    }

    public class DeleteAttendanceResponse
    {
        public string? AttendanceId { get; set; }
    }

    public class GetAllAttendanceItem
    {
        public string? Id { get; set; }
        public string? EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string? Status { get; set; }
        public double? TotalHours { get; set; }
        public double? OvertimeHours { get; set; }
        public UserBase? UserContext { get; set; }
    }

    public class GetAllAttendanceResponse
    {
        public List<GetAllAttendanceItem>? AttendanceRecords { get; set; }
    }
}
