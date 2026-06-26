using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stackoverflow.Domain.Entities
{
    public class Vote
    {
        public Guid VoteId { get; set; }

        public Guid UserId { get; set; }

        public Guid? QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        public VoteType VoteOn { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [NotMapped]
        public ICollection<Question> Questions { get; set; } = [];

        [NotMapped]
        public ICollection<Answer> Answers { get; set; } = [];

        [NotMapped]
        public UserProfile userprofiles { get; set; }

    }
}
