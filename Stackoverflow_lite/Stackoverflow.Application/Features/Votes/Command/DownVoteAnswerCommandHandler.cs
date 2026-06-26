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
    public class DownVoteAnswerCommandHandler
         : IRequestHandler<DownVoteAnswerCommand, ErrorOr<Success>>
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DownVoteAnswerCommandHandler(
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
            DownVoteAnswerCommand request,
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
            {
                return Error.Validation(
                        "Vote.Invalid",
                        "Cannot vote own answer");
            }
               

            var existingVote = await _voteRepository
                .GetAnswerVoteAsync(
                    userId,
                    request.AnswerId,
                    cancellationToken);


            var profile = await _profileRepository
                .GetByUserIdAsync(
                    answer.UserId,
                    cancellationToken);


            if ((existingVote != null) && (existingVote.VoteOn == VoteType.DownVote))
            {
                return Error.Validation(
                            "Vote.Exists",
                           "Already downvoted");

            }

            if (existingVote != null)
            {
                existingVote.VoteOn = VoteType.DownVote;
                if (profile!.ReputationScore > 0) profile!.ReputationScore -= 2;
                if (profile!.ReputationScore < 0) profile!.ReputationScore = 0;
            }

            else
            {
                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    UserId = userId,
                    AnswerId = request.AnswerId,
                    VoteOn = VoteType.DownVote,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _voteRepository
                    .AddAsync(vote, cancellationToken);

                if (profile!.ReputationScore > 0) profile!.ReputationScore -= 2;
                if (profile!.ReputationScore < 0) profile!.ReputationScore = 0;
            }

            _profileRepository.Update(profile);

            await _voteRepository
                .SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
