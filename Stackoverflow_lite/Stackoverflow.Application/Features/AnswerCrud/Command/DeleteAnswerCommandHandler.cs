using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using static Stackoverflow.Application.Contracts.IAnswerRepository;

namespace Stackoverflow.Application.Features.AnswerCrud.Command
{


    public class DeleteAnswerCommandHandler
        : IRequestHandler<DeleteAnswerCommand, ErrorOr<Deleted>>
    {
        private readonly IAnswerRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DeleteAnswerCommandHandler(
            IAnswerRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Deleted>> Handle(
            DeleteAnswerCommand request,
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

            if (string.IsNullOrEmpty(userId))
            {
                return Error.Unauthorized(
                    "Auth.Unauthorized",
                    "User is not authenticated");
            }

            if (answer.UserId != Guid.Parse(userId!))
            {
                return Error.Unauthorized(
                    "Answer.Unauthorized",
                    "You can only delete your own answer");
            }

            await _repository.DeleteVotesByAnswerIdAsync(
     answer.AnswerId,
     cancellationToken);

            await _repository
                .DeleteAsync(answer, cancellationToken);

            await _repository
                .SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
