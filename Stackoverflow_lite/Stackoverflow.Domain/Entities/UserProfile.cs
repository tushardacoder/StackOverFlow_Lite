using Stackoverflow.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stackoverflow.Domain.Entities
{
    public class UserProfile : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }


        public int ReputationScore { get; set; }


        [NotMapped]
        public ICollection<Question> Questions{ get; set; } = [];

        [NotMapped]
        public ICollection<Answer> Answers { get; set; } = [];
        [NotMapped]
        public Vote Votes { get; set; } 
      
    }
}
