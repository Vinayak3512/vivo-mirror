using AutoMapper;
using HRMS.Core.Telemetry.Exceptions;
using HRMS.Shared.Application.Constants;
using HRMS.Shared.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using PayrollFeature.Application.Repository;
using PayrollFeature.Domain;

namespace PayrollFeature.Application.DTO
{
    public class CreatePayrollHandler : IRequestHandler<CreatePayrollRequest, BaseResponse<CreatePayrollResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPayrollRepository _payrollRepository;

        public CreatePayrollHandler(IMapper mapper, IPayrollRepository payrollRepository)
        {
            _mapper = mapper;
            _payrollRepository = payrollRepository;
        }

        public async Task<BaseResponse<CreatePayrollResponse>> Handle(CreatePayrollRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<CreatePayrollResponse>();
            var payroll = _mapper.Map<PayrollRecord>(request.RequestParam);
            payroll = await _payrollRepository.AddItemAsync(payroll);

            if (payroll != null)
            {
                response.Data = new CreatePayrollResponse { PayrollId = payroll.Id };
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = string.Format(Messaging.Insert, nameof(PayrollRecord));
                response.Success = true;
            }

            return response;
        }
    }

    public sealed class GetAllPayrollsHandler : IRequestHandler<GetAllPayrollsRequest, BaseResponsePagination<GetAllPayrollsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPayrollRepository _payrollRepository;

        public GetAllPayrollsHandler(IPayrollRepository payrollRepository, IMapper mapper)
        {
            _mapper = mapper;
            _payrollRepository = payrollRepository;
        }

        public async Task<BaseResponsePagination<GetAllPayrollsResponse>> Handle(GetAllPayrollsRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponsePagination<GetAllPayrollsResponse>();
            (var payrolls, int count) = await _payrollRepository.GetAllPayrollsWithCountAsync(request);

            if (payrolls != null && payrolls.Any())
            {
                var data = _mapper.Map<IReadOnlyList<PayrollRecord>, IReadOnlyList<PayrollDto>>(payrolls.ToList());
                response.Data = new GetAllPayrollsResponse { Payrolls = data.ToList() };

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

    public sealed class UpdatePayrollHandler : IRequestHandler<UpdatePayrollRequest, BaseResponse<UpdatePayrollResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPayrollRepository _payrollRepository;

        public UpdatePayrollHandler(IMapper mapper, IPayrollRepository payrollRepository)
        {
            _mapper = mapper;
            _payrollRepository = payrollRepository;
        }

        public async Task<BaseResponse<UpdatePayrollResponse>> Handle(UpdatePayrollRequest request, CancellationToken cancellationToken)
        {
            if (request?.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var existing = await _payrollRepository.GetPayrollAsync(new GetAllPayrollsRequest
            {
                RequestParam = new PayrollDto { PayrollId = request.RequestParam.PayrollId }
            });

            if (existing == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(PayrollRecord)));

            var payroll = _mapper.Map<PayrollRecord>(request.RequestParam);
            payroll.UserContext = existing.UserContext;
            payroll.CreatedOn = existing.CreatedOn;
            payroll.CreatedByUserId = existing.CreatedByUserId;
            payroll.CreatedByUserName = existing.CreatedByUserName;

            await _payrollRepository.UpdateItemAsync(existing.Id, payroll);

            return new BaseResponse<UpdatePayrollResponse>
            {
                Data = new UpdatePayrollResponse { PayrollId = existing.Id },
                StatusCode = StatusCodes.Status200OK,
                Message = string.Format(Messaging.Update, nameof(PayrollRecord)),
                Success = true
            };
        }
    }

    public sealed class DeletePayrollHandler : IRequestHandler<DeletePayrollRequest, BaseResponse<DeletePayrollResponse>>
    {
        private readonly IPayrollRepository _payrollRepository;

        public DeletePayrollHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<BaseResponse<DeletePayrollResponse>> Handle(DeletePayrollRequest request, CancellationToken cancellationToken)
        {
            if (request?.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var existing = await _payrollRepository.GetPayrollAsync(new GetAllPayrollsRequest
            {
                RequestParam = new PayrollDto { PayrollId = request.RequestParam.PayrollId }
            });

            if (existing == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(PayrollRecord)));

            await _payrollRepository.DeleteItemAsync(existing.Id);

            return new BaseResponse<DeletePayrollResponse>
            {
                Data = new DeletePayrollResponse { PayrollId = existing.Id },
                StatusCode = StatusCodes.Status200OK,
                Message = string.Format(Messaging.Delete, nameof(PayrollRecord)),
                Success = true
            };
        }
    }
}
