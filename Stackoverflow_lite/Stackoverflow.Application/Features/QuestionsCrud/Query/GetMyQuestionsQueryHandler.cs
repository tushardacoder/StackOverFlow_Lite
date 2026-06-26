using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{
    public class GetMyQuestionsQueryHandler
     : IRequestHandler<GetMyQuestionsQuery, List<QuestionDto>>
    {
        private readonly IQuestionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyQuestionsQueryHandler(
            IQuestionRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<QuestionDto>> Handle(
            GetMyQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            //if (string.IsNullOrEmpty(userId))
            //    throw new Exception("Unauthorized");

            var questions = await _repository.GetAllAsync(cancellationToken);

            return questions
                .Where(x => x.UserId == Guid.Parse(userId))
                .Select(x => new QuestionDto
                {
                    QuestionId = x.QuestionId,
                    Title = x.Title,
                    Description = x.Description,
                    TagName = x.TagName,
                    CreatedAt = x.CreatedAt
                })
                .ToList();
        }
    }
}
