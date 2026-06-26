using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Votes.Command
{
    public class UpVoteAnswerCommand : IRequest<ErrorOr<Success>>
    {
        public Guid AnswerId { get; set; }
    }
}
