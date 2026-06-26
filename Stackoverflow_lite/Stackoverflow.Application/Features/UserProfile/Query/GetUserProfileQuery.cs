using ErrorOr;
using MediatR;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.UserProfile.Query
{
  

    public class GetUserProfileQuery : IRequest<ErrorOr<UserProfileDto>>
    {
        public Guid UserId { get; set; }
    }
}
