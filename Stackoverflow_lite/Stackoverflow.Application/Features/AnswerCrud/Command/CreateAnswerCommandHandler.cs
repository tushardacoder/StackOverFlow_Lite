using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Command
{

    public class CreateAnswerCommandHandler
        : IRequestHandler<CreateAnswerCommand, ErrorOr<Guid>>
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateAnswerCommandHandler(
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Guid>> Handle(
            CreateAnswerCommand request,
            CancellationToken cancellationToken)
        {
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


            var question = await _questionRepository
                .GetByIdAsync(
                    request.QuestionId,
                    cancellationToken);

            if (question == null)
            {
                return Error.NotFound(
                    "Question.NotFound",
                    "Question not found");
            }

            // Prevent answering own question
            if (question.UserId == Guid.Parse(userId!))
            {
                return Error.Validation(
                    "Answer.SelfAnswer",
                    "You cannot answer your own question");
            }

            var answer = new Answer
            {
                AnswerId = Guid.NewGuid(),
                QuestionId = request.QuestionId,
                UserId = Guid.Parse(userId!),
                Content = request.Content,
                IsAccepted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _answerRepository
                .AddAsync(answer, cancellationToken);

            await _answerRepository
                .SaveChangesAsync(cancellationToken);

            return answer.AnswerId;
        }
    }
}
