using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Application.Features.AnswerCrud.Command;
using Stackoverflow.Application.Features.AnswerCrud.Query;
using Stackoverflow.Application.Features.QuestionsCrud.Command;

namespace Stackoverflow.Host.Controllers
{


    [ApiController]
    [Route("api/answers")]
    public class AnswersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnswersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateAnswerCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("available-for-answer")]
        public async Task<IActionResult> GetAvailableQuestions()
        {
            var result = await _mediator.Send(
                new GetAvailableQuestionsQuery());

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> Update(
      UpdateAnswerCommand command)
        {
            
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
           
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAnswers()
        {
            var result = await _mediator.Send(new GetMyAnswersQuery());
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(
                new DeleteAnswerCommand
                {
                    AnswerId = id
                });


            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok();

            
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetByQuestionId(
        Guid questionId)
        {
            return Ok(await _mediator.Send(
                new GetAnswersByQuestionIdQuery
                {
                    QuestionId = questionId
                }));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("accept-answer")]
        public async Task<IActionResult> AcceptAnswer(
       AcceptAnswerCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                Message = "Answer accepted successfully."
            });
        }

    }
}
