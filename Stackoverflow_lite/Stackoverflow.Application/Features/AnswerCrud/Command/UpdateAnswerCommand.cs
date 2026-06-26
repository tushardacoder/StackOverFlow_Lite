using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Command
{
    public class UpdateAnswerCommand : IRequest<ErrorOr<Updated>>
    {
        public Guid AnswerId { get; set; }

        public string Content { get; set; }
    }
}
