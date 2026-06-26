using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.DTOS
{
    public class AnswerResponseDto
    {
        public string Message { get; set; } = "";
        public AnswerDto? AcceptedAnswer { get; set; }
        public List<AnswerDto> OtherAnswers { get; set; } = new();
    }
}
