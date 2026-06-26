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
    public class DownVoteQuestionCommandHandler
       : IRequestHandler<DownVoteQuestionCommand, ErrorOr<Success>>
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DownVoteQuestionCommandHandler(
            IVoteRepository voteRepository,
            IQuestionRepository questionRepository,
            IUserProfileRepository profileRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _voteRepository = voteRepository;
            _questionRepository = questionRepository;
            _profileRepository = profileRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Success>> Handle(
            DownVoteQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(
                _httpContextAccessor.HttpContext!
                .User
                .FindFirst("userid")!
                .Value);

            var question = await _questionRepository
                .GetByIdAsync(
                    request.QuestionId,
                    cancellationToken);

            if (question == null)
            {
                return Error.NotFound(
                     "Question.NotFound",
                       "Question not found");
            }


            if (question.UserId == userId)
            {
                return Error.Validation(
                           "Vote.Invalid",
                           "Cannot vote own question");
            }


            var existingVote = await _voteRepository
                .GetQuestionVoteAsync(
                    userId,
                    request.QuestionId,
                    cancellationToken);

            var profile = await _profileRepository
              .GetByUserIdAsync(
                  question.UserId,
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
                if (profile!.ReputationScore > 0) profile!.ReputationScore -= 1;
                if (profile!.ReputationScore < 0) profile!.ReputationScore = 0;


            }

            else
            {
                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    UserId = userId,
                    QuestionId = request.QuestionId,
                    VoteOn = VoteType.DownVote,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _voteRepository
                    .AddAsync(vote, cancellationToken);



                if (profile!.ReputationScore > 0) profile!.ReputationScore -= 1;
                if (profile!.ReputationScore < 0) profile!.ReputationScore = 0;



            }

            _profileRepository.Update(profile);
            await _voteRepository
                .SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
