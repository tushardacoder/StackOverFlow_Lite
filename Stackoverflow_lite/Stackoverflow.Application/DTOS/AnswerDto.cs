using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.DTOS
{
    public class AnswerDto
    {
        public Guid AnswerId { get; set; }

        public Guid QuestionId { get; set; }

        public string Content { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
