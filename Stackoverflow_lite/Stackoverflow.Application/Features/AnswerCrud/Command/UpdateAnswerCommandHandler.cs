using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Command
{
    public class UpdateAnswerCommandHandler
     : IRequestHandler<UpdateAnswerCommand, ErrorOr<Updated>>
    {
        private readonly IAnswerRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateAnswerCommandHandler(
            IAnswerRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Updated>> Handle(
            UpdateAnswerCommand request,
            CancellationToken cancellationToken)
        {
            var answer = await _repository
                .GetByIdAsync(
                    request.AnswerId,
                    cancellationToken);

            if (answer == null)
            {
                return Error.NotFound(
                    "Answer.NotFound",
                    "Answer not found");
            }

            var userId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            if (answer.UserId != Guid.Parse(userId!))
            {
                return Error.Unauthorized(
                    "Answer.Unauthorized",
                    "You can only update your own answer");
            }

            answer.Content = request.Content;
            answer.UpdatedAt = DateTime.UtcNow;

            await _repository
                .UpdateAsync(answer, cancellationToken);

            await _repository
                .SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }
}
