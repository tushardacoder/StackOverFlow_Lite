using MediatR;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{
    
       

public class GetAllQuestionsQueryHandler
    : IRequestHandler<GetAllQuestionsQuery, List<QuestionDto>>
    {
        private readonly IQuestionRepository _repository;

        public GetAllQuestionsQueryHandler(
            IQuestionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<QuestionDto>> Handle(
            GetAllQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var questions = await _repository
                .GetAllAsync(cancellationToken);

            return questions.Select(x => new QuestionDto
        {
            QuestionId = x.QuestionId,
            Title = x.Title,
            Description = x.Description,
            TagName = x.TagName,
            AcceptedAnswer = x.AcceptedAnswer,
            CreatedAt = x.CreatedAt
        })
        .ToList();

        }
    }
}

