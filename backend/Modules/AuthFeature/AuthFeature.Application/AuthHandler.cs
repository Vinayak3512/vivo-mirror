using AuthFeature.Application.DTO;
using AuthFeature.Application.Services;
using HRMS.Shared.Application.DTOs;
using MediatR;
using UserFeature.Application.Repository;
using UserFeature.Domain;
using BCrypt.Net;

namespace AuthFeature.Application
{
    public class AuthHandler : 
        IRequestHandler<LoginRequest, BaseResponse<LoginResponseDto>>,
        IRequestHandler<RefreshRequest, BaseResponse<LoginResponseDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtService _jwtService;

        public AuthHandler(IEmployeeRepository employeeRepository, IJwtService jwtService)
        {
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
        }

        public async Task<BaseResponse<LoginResponseDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByEmailAsync(request.RequestParam.Email);
            
            if (employee == null || string.IsNullOrEmpty(employee.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.RequestParam.Password, employee.PasswordHash))
            {
                return new BaseResponse<LoginResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    StatusCode = 401
                };
            }

            var accessToken = _jwtService.GenerateAccessToken(employee);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new BaseResponse<LoginResponseDto>
            {
                Success = true,
                Data = new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = new UserDto
                    {
                        Id = employee.Id,
                        Name = $"{employee.FirstName} {employee.LastName}",
                        Email = employee.Email ?? "",
                        Role = employee.Role ?? "Employee"
                    }
                },
                StatusCode = 200
            };
        }

        public async Task<BaseResponse<LoginResponseDto>> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            // For MVP, we'll just return a new token if a refresh token is provided
            // In a real app, you'd validate the refresh token against a store
            return new BaseResponse<LoginResponseDto>
            {
                Success = false,
                Message = "Refresh token not implemented in MVP",
                StatusCode = 501
            };
        }
    }
}
