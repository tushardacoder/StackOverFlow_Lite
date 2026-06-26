using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Application.Features.AnswerCrud.Command;
using Stackoverflow.Application.Features.AnswerCrud.Query;

using Stackoverflow.Application.Features.QuestionsCrud.Query;
using Stackoverflow.Application.Features.Votes.Command;
using GetAvailableQuestionsQuery = Stackoverflow.Application.Features.AnswerCrud.Query.GetAvailableQuestionsQuery;

namespace Stackoverflow.Host.Controllers
{
    [ApiController]
    [Route("api/votes")]
    public class VotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("available-answer-for-vote")]
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
        [HttpGet("myquestion")]
        public async Task<IActionResult> GetMyQuestions()
        {
            var result = await _mediator.Send(new GetMyQuestionsQuery());
            return Ok(result);
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
        [HttpPost("question/upvote")]
        public async Task<IActionResult> UpVoteQuestion(
    UpVoteQuestionCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }


            return Ok();
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("question/downvote")]
        public async Task<IActionResult> DownVoteQuestion(
            DownVoteQuestionCommand command)
        {
            var result = await _mediator.Send(command);


            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }


            return Ok();

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("answer/upvote")]
        public async Task<IActionResult> UpVoteAnswer(
            UpVoteAnswerCommand command)
        {
           var result= await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }


            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("answer/downvote")]
        public async Task<IActionResult> DownVoteAnswer(
            DownVoteAnswerCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }


            return Ok();
        }
    }
}
