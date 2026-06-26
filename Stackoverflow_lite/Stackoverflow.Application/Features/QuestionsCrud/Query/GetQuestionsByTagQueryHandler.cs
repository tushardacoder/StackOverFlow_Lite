using MediatR;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{
    public class GetQuestionsByTagQueryHandler
    : IRequestHandler<GetQuestionsByTagQuery, List<QuestionDto>>
    {
        private readonly IQuestionRepository _repository;

        public GetQuestionsByTagQueryHandler(IQuestionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<QuestionDto>> Handle(
            GetQuestionsByTagQuery request,
            CancellationToken cancellationToken)
        {
            var questions = await _repository.GetByTagAsync(
                request.TagName,
                cancellationToken);

            return questions 
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
