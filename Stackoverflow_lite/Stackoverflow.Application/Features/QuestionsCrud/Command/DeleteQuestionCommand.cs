using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{
    public class DeleteQuestionCommand : IRequest<ErrorOr<Success>>
    {
        public Guid QuestionId { get; set; }
    }
}
