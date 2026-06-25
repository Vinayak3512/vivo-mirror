using AuthFeature.Application.DTO;
using UserFeature.Domain;

namespace AuthFeature.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(Employee employee);
        string GenerateRefreshToken();
    }
}
