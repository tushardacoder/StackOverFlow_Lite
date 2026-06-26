using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Command
{
    public class CreateAnswerCommand : IRequest<ErrorOr<Guid>>
    {
        public Guid QuestionId { get; set; }

        public string Content { get; set; }
    }
}
