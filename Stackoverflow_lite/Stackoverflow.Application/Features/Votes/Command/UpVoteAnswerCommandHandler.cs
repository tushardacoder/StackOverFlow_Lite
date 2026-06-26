using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Votes.Command
{
    public class UpVoteAnswerCommandHandler
       : IRequestHandler<UpVoteAnswerCommand, ErrorOr<Success>>
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpVoteAnswerCommandHandler(
            IVoteRepository voteRepository,
            IAnswerRepository answerRepository,
            IUserProfileRepository profileRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _voteRepository = voteRepository;
            _answerRepository = answerRepository;
            _profileRepository = profileRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Success>> Handle(
            UpVoteAnswerCommand request,
            CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(
                _httpContextAccessor.HttpContext!
                .User
                .FindFirst("userid")!
                .Value);

         

            var answer = await _answerRepository
                .GetByIdAsync(
                    request.AnswerId,
                    cancellationToken);

            if (answer == null)
            {
                return Error.NotFound(
                     "Answer.NotFound",
                       "Answer not found");
            }

            if (answer.UserId == userId)
                return Error.Validation("Cannot vote own answer");

            var existingVote = await _voteRepository
                .GetAnswerVoteAsync(
                    userId,
                    request.AnswerId,
                    cancellationToken);

            var profile = await _profileRepository
               .GetByUserIdAsync(
                   answer.UserId,
                   cancellationToken);

            if((existingVote != null)&& (existingVote.VoteOn== VoteType.UpVote))
            {
                return Error.Validation(
              "Vote.Exists",
             "Already Upvoted");
              

            }

           else if (existingVote != null)
            {
                existingVote.VoteOn = VoteType.UpVote;
                profile!.ReputationScore += 10;
            }

            else
            {
                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    UserId = userId,
                    AnswerId = request.AnswerId,
                    VoteOn = VoteType.UpVote,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _voteRepository
                    .AddAsync(vote, cancellationToken);



                profile!.ReputationScore += 10;
            }
            
            _profileRepository.Update(profile);

            await _voteRepository
                .SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
