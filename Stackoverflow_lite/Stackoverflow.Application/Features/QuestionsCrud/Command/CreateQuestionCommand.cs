using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{
    

    public class CreateQuestionCommand : IRequest<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string TagName { get; set; }
    }
}
