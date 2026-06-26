using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.DTOS
{
    public class QuestionDto
    {
        public Guid QuestionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TagName { get; set; }

        public bool AcceptedAnswer { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
