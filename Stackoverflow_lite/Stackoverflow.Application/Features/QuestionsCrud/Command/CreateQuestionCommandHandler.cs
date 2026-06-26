using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Stackoverflow.Application.Contracts;
    using Stackoverflow.Domain.Entities;

    public class CreateQuestionCommandHandler
        : IRequestHandler<CreateQuestionCommand, Guid>
    {
        private readonly IQuestionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateQuestionCommandHandler(
            IQuestionRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(
            CreateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            var question = new Question
            {
                QuestionId = Guid.NewGuid(),
                UserId = Guid.Parse(userId!),
                Title = request.Title,
                Description = request.Description,
                TagName = request.TagName,
                AcceptedAnswer = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(question, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);


            return question.QuestionId;
        }
    }
}
