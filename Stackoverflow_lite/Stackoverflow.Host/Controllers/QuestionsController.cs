namespace Stackoverflow.Host.Controllers
{
    using ErrorOr;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Stackoverflow.Application.Contracts;
    using Stackoverflow.Application.Features.AnswerCrud.Query;
    using Stackoverflow.Application.Features.QuestionsCrud.Command;
    using Stackoverflow.Application.Features.QuestionsCrud.Query;

    [ApiController]
    [Route("api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IViewTrackingService _viewTrackingService;
       
        public QuestionsController(IMediator mediator, IViewTrackingService viewTrackingService)
        {
            _mediator = mediator;
            _viewTrackingService = viewTrackingService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateQuestionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _mediator.Send(
                             new GetAllQuestionsQuery());

            return Ok(questions);
            
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetQuestionByIdQuery
                {
                    QuestionId = id
                }));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("tag/{tagName}")]
        public async Task<IActionResult> GetByTag(string tagName)
        {
            var result = await _mediator.Send(
                new GetQuestionsByTagQuery
                {
                    TagName = tagName
                });

            return Ok(result);
        }

        

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyQuestions()
        {
            var result = await _mediator.Send(new GetMyQuestionsQuery());
            return Ok(result);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateQuestionCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                Message = "Question updated successfully."
            });
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(
                new DeleteQuestionCommand
                {
                    QuestionId = id
                });

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                Message = "Question deleted successfully."
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("accept-answer")]
        public async Task<IActionResult> AcceptAnswer(
       AcceptAnswerCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet("{questionId}/views")]
        public async Task<IActionResult> GetViews(
    Guid questionId)
        {
            var views = await _viewTrackingService
                .GetQuestionViewsAsync(questionId);

            return Ok(views);
        }


    }
}
