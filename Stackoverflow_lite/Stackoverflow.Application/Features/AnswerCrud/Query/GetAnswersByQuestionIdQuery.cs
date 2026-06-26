using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Stackoverflow.Application.DTOS;

namespace Stackoverflow.Application.Features.AnswerCrud.Query
{
  

    public class GetAnswersByQuestionIdQuery
        : IRequest<AnswerResponseDto>
    {
        public Guid QuestionId { get; set; }
    }
}
