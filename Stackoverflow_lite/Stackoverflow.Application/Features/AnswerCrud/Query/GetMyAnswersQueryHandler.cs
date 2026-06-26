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
    public class GetMyAnswersQueryHandler
    : IRequestHandler<
        GetMyAnswersQuery,
        ErrorOr<List<AnswerDto>>>
    {
        private readonly IAnswerRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyAnswersQueryHandler(
            IAnswerRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<List<AnswerDto>>> Handle(
            GetMyAnswersQuery request,
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

            var answers = await _repository.GetByUserIdAsync(
                Guid.Parse(userId),
                cancellationToken) ?? new List<Answer>();

            return answers.Select(x => new AnswerDto
            {
                AnswerId = x.AnswerId,
                QuestionId = x.QuestionId,
                Content = x.Content,
                IsAccepted = x.IsAccepted,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}
