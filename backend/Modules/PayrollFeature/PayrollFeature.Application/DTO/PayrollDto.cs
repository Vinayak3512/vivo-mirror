using HRMS.Shared.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace PayrollFeature.Application.DTO
{
    public class PayrollDto
    {
        public string? PayrollId { get; set; }
        public string? UserId { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal GrossPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public string? Currency { get; set; }
        public string? CountryCode { get; set; }
        public string? Status { get; set; }
        public string? PayslipUrl { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal Bonus { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Reimbursements { get; set; }

        public decimal PF { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal ESI { get; set; }
        public decimal HealthInsurance { get; set; }
        public decimal LWF { get; set; }
        public decimal OtherDeductions { get; set; }

        public decimal EmployerPF { get; set; }
        public decimal EmployerESI { get; set; }
        public decimal Gratuity { get; set; }
        public string? Keyword { get; set; }
    }

    public class CreatePayrollRequest : IRequest<BaseResponse<CreatePayrollResponse>>
    {
        public PayrollDto? RequestParam { get; set; }
    }

    public class CreatePayrollResponse
    {
        public string? PayrollId { get; set; }
    }

    public class GetAllPayrollsRequest : BaseRequestPagination, IRequest<BaseResponsePagination<GetAllPayrollsResponse>>
    {
        public PayrollDto? RequestParam { get; set; }
    }

    public class GetAllPayrollsResponse
    {
        public List<PayrollDto>? Payrolls { get; set; }
    }

    public class UpdatePayrollRequest : IRequest<BaseResponse<UpdatePayrollResponse>>
    {
        public PayrollDto? RequestParam { get; set; }
    }

    public class UpdatePayrollResponse
    {
        public string? PayrollId { get; set; }
    }

    public class DeletePayrollRequest : IRequest<BaseResponse<DeletePayrollResponse>>
    {
        public PayrollDto? RequestParam { get; set; }
    }

    public class DeletePayrollResponse
    {
        public string? PayrollId { get; set; }
    }
}
