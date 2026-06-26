using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stackoverflow.Domain.Entities
{
    public class Question
    {
        public Guid QuestionId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


        public string TagName { get; set; }

        public bool AcceptedAnswer { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        [NotMapped]
        public ICollection<UserProfile> Usersprofiles { get; set; } = [];



        [NotMapped]
        public ICollection<Answer> Answers { get; set; } = [];



        [NotMapped]
        public ICollection<Vote> Votes { get; set; } = [];

    }
}
