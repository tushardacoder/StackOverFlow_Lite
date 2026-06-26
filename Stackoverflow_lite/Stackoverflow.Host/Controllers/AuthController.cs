using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Application.Features.Auth.Command;

namespace Stackoverflow.Host.Controllers
{


    [ApiController]
    [Route("api/auth")]

    public class AuthController : ControllerBase
    {


        private readonly IMediator _mediator;



        public AuthController(IMediator mediator)
        {

            _mediator = mediator;

        }





        [HttpPost("register")]
        public async Task<IActionResult> Register(
     UserRegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                UserId = result.Value
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
     UserLoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
            
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet("getquestion")]
        //public IActionResult CreateQuestion()
        //{

        //    var user = User.Identity?.IsAuthenticated;

        //    return Ok(user);
        //}

    }


}
