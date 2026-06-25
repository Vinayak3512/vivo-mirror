using AutoMapper;
using HRMS.Core.Telemetry.Exceptions;
using HRMS.Shared.Application.Constants;
using HRMS.Shared.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamFeature.Application.Repository;
using TeamFeature.Domain;

namespace TeamFeature.Application.DTO
{
    public class CreateTeamMemberHandler : IRequestHandler<CreateTeamMemberRequest, BaseResponse<CreateTeamMemberResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public CreateTeamMemberHandler(IMapper mapper, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
        }

        public async Task<BaseResponse<CreateTeamMemberResponse>> Handle(CreateTeamMemberRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<CreateTeamMemberResponse>();
            var teamMember = _mapper.Map<TeamMember>(request.RequestParam);
            teamMember = await _teamRepository.AddItemAsync(teamMember);

            if (teamMember != null)
            {
                response.Data = new CreateTeamMemberResponse { MemberId = teamMember.Id };
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = string.Format(Messaging.Insert, nameof(TeamMember));
                response.Success = true;
            }

            return response;
        }
    }

    public sealed class GetAllTeamMembersHandler : IRequestHandler<GetAllTeamMembersRequest, BaseResponsePagination<GetAllTeamMembersResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public GetAllTeamMembersHandler(ITeamRepository teamRepository, IMapper mapper)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
        }

        public async Task<BaseResponsePagination<GetAllTeamMembersResponse>> Handle(GetAllTeamMembersRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponsePagination<GetAllTeamMembersResponse>();
            (var teamMembers, int count) = await _teamRepository.GetAllTeamMembersWithCountAsync(request);

            if (teamMembers != null && teamMembers.Any())
            {
                var data = _mapper.Map<IReadOnlyList<TeamMember>, IReadOnlyList<TeamMemberDto>>(teamMembers.ToList());
                response.Data = new GetAllTeamMembersResponse { TeamMembers = data.ToList() };

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

    public sealed class UpdateTeamMemberHandler : IRequestHandler<UpdateTeamMemberRequest, BaseResponse<UpdateTeamMemberResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public UpdateTeamMemberHandler(IMapper mapper, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
        }

        public async Task<BaseResponse<UpdateTeamMemberResponse>> Handle(UpdateTeamMemberRequest request, CancellationToken cancellationToken)
        {
            if (request?.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var existing = await _teamRepository.GetTeamMemberAsync(new GetAllTeamMembersRequest
            {
                RequestParam = new TeamMemberDto { MemberId = request.RequestParam.MemberId }
            });

            if (existing == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(TeamMember)));

            var teamMember = _mapper.Map<TeamMember>(request.RequestParam);
            teamMember.UserContext = existing.UserContext;
            teamMember.CreatedOn = existing.CreatedOn;
            teamMember.CreatedByUserId = existing.CreatedByUserId;
            teamMember.CreatedByUserName = existing.CreatedByUserName;

            await _teamRepository.UpdateItemAsync(existing.Id, teamMember);

            return new BaseResponse<UpdateTeamMemberResponse>
            {
                Data = new UpdateTeamMemberResponse { MemberId = existing.Id },
                StatusCode = StatusCodes.Status200OK,
                Message = string.Format(Messaging.Update, nameof(TeamMember)),
                Success = true
            };
        }
    }

    public sealed class DeleteTeamMemberHandler : IRequestHandler<DeleteTeamMemberRequest, BaseResponse<DeleteTeamMemberResponse>>
    {
        private readonly ITeamRepository _teamRepository;

        public DeleteTeamMemberHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<BaseResponse<DeleteTeamMemberResponse>> Handle(DeleteTeamMemberRequest request, CancellationToken cancellationToken)
        {
            if (request?.RequestParam == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var existing = await _teamRepository.GetTeamMemberAsync(new GetAllTeamMembersRequest
            {
                RequestParam = new TeamMemberDto { MemberId = request.RequestParam.MemberId }
            });

            if (existing == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(TeamMember)));

            await _teamRepository.DeleteItemAsync(existing.Id);

            return new BaseResponse<DeleteTeamMemberResponse>
            {
                Data = new DeleteTeamMemberResponse { MemberId = existing.Id },
                StatusCode = StatusCodes.Status200OK,
                Message = string.Format(Messaging.Delete, nameof(TeamMember)),
                Success = true
            };
        }
    }
}
