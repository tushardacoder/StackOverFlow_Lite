using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{
    public class UpdateQuestionCommand : IRequest<ErrorOr<Success>>
    {
        public Guid QuestionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TagName { get; set; }
    }
}
