using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Application.Features.UserProfile.Query;

namespace Stackoverflow.Host.Controllers
{
   

     [ApiController]
    [Route("api/userprofile")]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirst("userid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _mediator.Send(
                new GetUserProfileQuery
                {
                    UserId = Guid.Parse(userId)
                });

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }

}
