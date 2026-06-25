using HRMS.Shared.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace ExpensesFeature.Application.DTO
{
    public class ExpenseDto
    {
        public string? ExpenseId { get; set; }
        public string? UserId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? ReceiptUrl { get; set; }
        public string? Status { get; set; }
        public string? ApprovedByUserId { get; set; }
        public string? RejectionReason { get; set; }
        public string? Keyword { get; set; }
    }

    public class CreateExpenseRequest : IRequest<BaseResponse<CreateExpenseResponse>>
    {
        public ExpenseDto? RequestParam { get; set; }
    }

    public class CreateExpenseResponse
    {
        public string? ExpenseId { get; set; }
    }

    public class GetAllExpensesRequest : BaseRequestPagination, IRequest<BaseResponsePagination<GetAllExpensesResponse>>
    {
        public ExpenseDto? RequestParam { get; set; }
    }

    public class GetAllExpensesResponse
    {
        public List<ExpenseDto>? Expenses { get; set; }
    }

    public class UpdateExpenseRequest : IRequest<BaseResponse<UpdateExpenseResponse>>
    {
        public ExpenseDto? RequestParam { get; set; }
    }

    public class UpdateExpenseResponse
    {
        public string? ExpenseId { get; set; }
    }

    public class DeleteExpenseRequest : IRequest<BaseResponse<DeleteExpenseResponse>>
    {
        public ExpenseDto? RequestParam { get; set; }
    }

    public class DeleteExpenseResponse
    {
        public string? ExpenseId { get; set; }
    }
}
