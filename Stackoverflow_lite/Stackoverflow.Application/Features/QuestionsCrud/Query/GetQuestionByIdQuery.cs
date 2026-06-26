using MediatR;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{

    public class GetQuestionByIdQuery
     : IRequest<QuestionDto>
    {
        public Guid QuestionId { get; set; }
    }
}
