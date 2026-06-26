using ErrorOr;
using MediatR;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.UserProfile.Query
{
 

    public class GetUserProfileQueryHandler
        : IRequestHandler<GetUserProfileQuery, ErrorOr<UserProfileDto>>
    {
        private readonly IUserProfileRepository _repository;

        public GetUserProfileQueryHandler(
            IUserProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<ErrorOr<UserProfileDto>> Handle(
            GetUserProfileQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByUserIdAsync(
                request.UserId,
                cancellationToken);

            if (profile == null)
            {
                
                  return Error.NotFound(
                       "Profile.NotFound",
                       "Profile not found");
                
            }
                

            return new UserProfileDto
            {
                UserId = profile.UserId,
                ReputationScore = profile.ReputationScore
            };
        }
    }

}
