using MediatR;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Stackoverflow.Application.Features.AnswerCrud.Query
{


    public class GetAnswersByQuestionIdQueryHandler
    : IRequestHandler<GetAnswersByQuestionIdQuery, AnswerResponseDto>
    {
        private readonly IAnswerRepository _repository;


        public GetAnswersByQuestionIdQueryHandler(
            IAnswerRepository repository)
        {
            _repository = repository;
        }


        public async Task<AnswerResponseDto> Handle(
            GetAnswersByQuestionIdQuery request,
            CancellationToken cancellationToken)
        {
            var answers = await _repository
                .GetByQuestionIdAsync(
                    request.QuestionId,
                    cancellationToken) ?? new List<Answer>(); 


            var acceptedAnswer = answers
                .Where(x => x.IsAccepted)
                .Select(x => new AnswerDto
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId,
                    Content = x.Content,
                    IsAccepted = x.IsAccepted,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefault();


            var otherAnswers = answers
                .Where(x => !x.IsAccepted)
                .Select(x => new AnswerDto
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId,
                    Content = x.Content,
                    IsAccepted = x.IsAccepted,
                    CreatedAt = x.CreatedAt
                })
                .ToList();


            return new AnswerResponseDto
            {
                Message = acceptedAnswer != null
                    ? "Accepted answer found"
                    : "No accepted answer yet",

                AcceptedAnswer = acceptedAnswer,

                OtherAnswers = otherAnswers
            };
        }
    }
}
