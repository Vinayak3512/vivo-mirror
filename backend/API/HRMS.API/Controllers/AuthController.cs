using AuthFeature.Application.DTO;
using HRMS.Shared.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _mediator.Send(new LoginRequest { RequestParam = request });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return Unauthorized(new { message = result.Message });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _mediator.Send(new RefreshRequest { RequestParam = request });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(new { message = result.Message });
        }
    }
}
