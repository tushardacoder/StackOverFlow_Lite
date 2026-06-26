using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{
    public class GetAvailableQuestionsQueryHandler
        : IRequestHandler<GetAvailableQuestionsQuery, List<QuestionDto>>
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


        public async Task<List<QuestionDto>> Handle(
            GetAvailableQuestionsQuery request,
            CancellationToken cancellationToken)
        {

            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("userid")?
                .Value;


            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");


            var questions = await _repository
                .GetQuestionsExceptUserAsync(
                    Guid.Parse(userId),
                    cancellationToken);



            return questions.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                Title = q.Title,
                Description = q.Description,
                TagName = q.TagName,
                AcceptedAnswer = q.AcceptedAnswer,
                CreatedAt = q.CreatedAt

            }).ToList();

        }
    }
}
