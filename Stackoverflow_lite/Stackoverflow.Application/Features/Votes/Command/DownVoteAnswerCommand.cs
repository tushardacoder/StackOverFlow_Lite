using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Votes.Command
{
    public class DownVoteAnswerCommand : IRequest<ErrorOr<Success>>
    {
        public Guid AnswerId { get; set; }
    }
}
