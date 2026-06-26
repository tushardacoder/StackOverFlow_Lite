using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Query
{
   

    public class GetAvailableQuestionsQueryHandler
        : IRequestHandler<
        GetAvailableQuestionsQuery,
        ErrorOr<List<QuestionDto>>>
    {
        private readonly IQuestionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAvailableQuestionsQueryHandler(
            IQuestionRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<List<QuestionDto>>> Handle(
            GetAvailableQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Error.Unauthorized(
                    "Auth.Unauthorized",
                    "User is not authenticated");
            }

            var questions = await _repository
                .GetQuestionsExceptUserAsync(
                    Guid.Parse(userId),
                    cancellationToken) ?? new List<Question>();

            return questions.Select(x => new QuestionDto
            {
                QuestionId = x.QuestionId,
                Title = x.Title,
                Description = x.Description,
                TagName = x.TagName,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}
