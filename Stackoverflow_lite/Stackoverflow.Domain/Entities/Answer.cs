using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stackoverflow.Domain.Entities
{
    public class Answer
    {
        public Guid AnswerId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; }

        public bool IsAccepted { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }



        [NotMapped]
        public ICollection<Question> Questions { get; set; } = [];


        [NotMapped]
        public ICollection<Vote> Votes { get; set; } = [];
    }
}
