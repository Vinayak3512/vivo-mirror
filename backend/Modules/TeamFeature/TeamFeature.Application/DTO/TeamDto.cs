using HRMS.Shared.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace TeamFeature.Application.DTO
{
    public class TeamMemberDto
    {
        public string? MemberId { get; set; }
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string? Status { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Keyword { get; set; }
    }

    public class CreateTeamMemberRequest : IRequest<BaseResponse<CreateTeamMemberResponse>>
    {
        public TeamMemberDto? RequestParam { get; set; }
    }

    public class CreateTeamMemberResponse
    {
        public string? MemberId { get; set; }
    }

    public class GetAllTeamMembersRequest : BaseRequestPagination, IRequest<BaseResponsePagination<GetAllTeamMembersResponse>>
    {
        public TeamMemberDto? RequestParam { get; set; }
    }

    public class GetAllTeamMembersResponse
    {
        public List<TeamMemberDto>? TeamMembers { get; set; }
    }

    public class UpdateTeamMemberRequest : IRequest<BaseResponse<UpdateTeamMemberResponse>>
    {
        public TeamMemberDto? RequestParam { get; set; }
    }

    public class UpdateTeamMemberResponse
    {
        public string? MemberId { get; set; }
    }

    public class DeleteTeamMemberRequest : IRequest<BaseResponse<DeleteTeamMemberResponse>>
    {
        public TeamMemberDto? RequestParam { get; set; }
    }

    public class DeleteTeamMemberResponse
    {
        public string? MemberId { get; set; }
    }
}
