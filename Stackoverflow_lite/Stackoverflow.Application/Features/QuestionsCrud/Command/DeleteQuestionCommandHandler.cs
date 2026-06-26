using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{
    public class DeleteQuestionCommandHandler
     : IRequestHandler<DeleteQuestionCommand, ErrorOr<Success>>
    {
        private readonly IQuestionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteQuestionCommandHandler(
            IQuestionRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Success>> Handle(
            DeleteQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question = await _repository
                .GetByIdAsync(request.QuestionId, cancellationToken);

            if (question is null)
            {
                return Error.NotFound(
                    code: "Question.NotFound",
                    description: "Question not found.");
            }

            var userId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            if (question.UserId != Guid.Parse(userId!))
            {
                return Error.Forbidden(
                    code: "Question.DeleteDenied",
                    description: "You can only delete your own question.");
            }


            await _repository.DeleteQuestionVotesAsync(
                        question.QuestionId,
                        cancellationToken);

            var answers = await _repository.GetAnswersByQuestionIdAsync(
                question.QuestionId,
                cancellationToken);

            foreach (var answer in answers)
            {
                await _repository.DeleteAnswerVotesAsync(
                    answer.AnswerId,
                    cancellationToken);
            }

            await _repository.DeleteAnswersByQuestionIdAsync(
                question.QuestionId,
                cancellationToken);

            await _repository.DeleteAsync(question, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
