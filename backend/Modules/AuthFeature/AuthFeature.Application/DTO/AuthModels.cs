using HRMS.Core.Postgres.Common;
using HRMS.Shared.Application.DTOs;
using MediatR;

namespace AuthFeature.Application.DTO
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class LoginRequest : ExecutionRequest, IRequest<BaseResponse<LoginResponseDto>>
    {
        public LoginRequestDto RequestParam { get; set; } = new();
    }

    public class RefreshRequest : ExecutionRequest, IRequest<BaseResponse<LoginResponseDto>>
    {
        public RefreshTokenRequestDto RequestParam { get; set; } = new();
    }
}
