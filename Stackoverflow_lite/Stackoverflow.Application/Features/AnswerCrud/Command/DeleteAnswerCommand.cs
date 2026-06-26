using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Command
{
    

    public class DeleteAnswerCommand : IRequest<ErrorOr<Deleted>>
    {
        public Guid AnswerId { get; set; }
    }
}
