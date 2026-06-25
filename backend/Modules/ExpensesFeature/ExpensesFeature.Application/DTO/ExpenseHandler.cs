using AutoMapper;
using HRMS.Core.Telemetry.Exceptions;
using HRMS.Shared.Application.Constants;
using HRMS.Shared.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using ExpensesFeature.Application.Repository;
using ExpensesFeature.Domain;

namespace ExpensesFeature.Application.DTO
{
    public class CreateExpenseHandler : IRequestHandler<CreateExpenseRequest, BaseResponse<CreateExpenseResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;

        public CreateExpenseHandler(IMapper mapper, IExpenseRepository expenseRepository)
        {
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        public async Task<BaseResponse<CreateExpenseResponse>> Handle(CreateExpenseRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<CreateExpenseResponse>();
            var expense = _mapper.Map<ExpenseRecord>(request.RequestParam);
            expense = await _expenseRepository.AddItemAsync(expense);

            if (expense != null)
            {
                response.Data = new CreateExpenseResponse { ExpenseId = expense.Id };
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = string.Format(Messaging.Insert, nameof(ExpenseRecord));
                response.Success = true;
            }

            return response;
        }
    }

    public sealed class GetAllExpensesHandler : IRequestHandler<GetAllExpensesRequest, BaseResponsePagination<GetAllExpensesResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;

        public GetAllExpensesHandler(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        public async Task<BaseResponsePagination<GetAllExpensesResponse>> Handle(GetAllExpensesRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponsePagination<GetAllExpensesResponse>();
            (var expenses, int count) = await _expenseRepository.GetAllExpensesWithCountAsync(request);

            if (expenses != null && expenses.Any())
            {
                var data = _mapper.Map<IReadOnlyList<ExpenseRecord>, IReadOnlyList<ExpenseDto>>(expenses.ToList());
                response.Data = new GetAllExpensesResponse { Expenses = data.ToList() };

                if (request.PageCriteria != null && request.PageCriteria.EnablePage)
                {
                    response.Meta = new Meta
                    {
                        Skip = request.PageCriteria.Skip,
                        Take = request.PageCriteria.PageSize,
                        TotalCount = count
                    };
                }
            }

            response.Success = true;
            response.StatusCode = StatusCodes.Status200OK;
            return response;
        }
    }

    public sealed class UpdateExpenseHandler : IRequestHandler<UpdateExpenseRequest, BaseResponse<UpdateExpenseResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;

        public UpdateExpenseHandler(IMapper mapper, IExpenseRepository expenseRepository)
        {
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        public async Task<BaseResponse<UpdateExpenseResponse>> Handle(UpdateExpenseRequest request, CancellationToken cancellationToken)
        {
            if (request?.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var existing = await _expenseRepository.GetExpenseAsync(new GetAllExpensesRequest
            {
                RequestParam = new ExpenseDto { ExpenseId = request.RequestParam.ExpenseId }
            });

            if (existing == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(ExpenseRecord)));

            var expense = _mapper.Map<ExpenseRecord>(request.RequestParam);
            expense.UserContext = existing.UserContext;
            expense.CreatedOn = existing.CreatedOn;
            expense.CreatedByUserId = existing.CreatedByUserId;
            expense.CreatedByUserName = existing.CreatedByUserName;

            await _expenseRepository.UpdateItemAsync(existing.Id, expense);

            return new BaseResponse<UpdateExpenseResponse>
            {
                Data = new UpdateExpenseResponse { ExpenseId = existing.Id },
                StatusCode = StatusCodes.Status200OK,
                Message = string.Format(Messaging.Update, nameof(ExpenseRecord)),
                Success = true
            };
        }
    }

    public sealed class DeleteExpenseHandler : IRequestHandler<DeleteExpenseRequest, BaseResponse<DeleteExpenseResponse>>
    {
        private readonly IExpenseRepository _expenseRepository;

        public DeleteExpenseHandler(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<BaseResponse<DeleteExpenseResponse>> Handle(DeleteExpenseRequest request, CancellationToken cancellationToken)
        {
            if (request?.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var existing = await _expenseRepository.GetExpenseAsync(new GetAllExpensesRequest
            {
                RequestParam = new ExpenseDto { ExpenseId = request.RequestParam.ExpenseId }
            });

            if (existing == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(ExpenseRecord)));

            await _expenseRepository.DeleteItemAsync(existing.Id);

            return new BaseResponse<DeleteExpenseResponse>
            {
                Data = new DeleteExpenseResponse { ExpenseId = existing.Id },
                StatusCode = StatusCodes.Status200OK,
                Message = string.Format(Messaging.Delete, nameof(ExpenseRecord)),
                Success = true
            };
        }
    }
}
