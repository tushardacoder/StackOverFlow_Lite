using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.DTOS
{
    public class UserProfileDto
    {
        public Guid UserId { get; set; }

        public int ReputationScore { get; set; }
    }
}
