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
    public class UpVoteQuestionCommandHandler
       : IRequestHandler<UpVoteQuestionCommand, ErrorOr<Success>>
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpVoteQuestionCommandHandler(
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
            UpVoteQuestionCommand request,
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
                return Error.Validation("Cannot vote own question");



            var existingVote = await _voteRepository
                .GetQuestionVoteAsync(
                    userId,
                    request.QuestionId,
                    cancellationToken);

            var profile = await _profileRepository
               .GetByUserIdAsync(
                   question.UserId,
                   cancellationToken);

            if ((existingVote != null) && (existingVote.VoteOn == VoteType.UpVote))
            {
                return Error.Validation(
               "Vote.Exists",
              "Already Upvoted");

            }

            if (existingVote != null)
            {
                existingVote.VoteOn = VoteType.UpVote;
                profile!.ReputationScore += 5;
            }

            else
            {
                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    UserId = userId,
                    QuestionId = request.QuestionId,
                    VoteOn = VoteType.UpVote,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _voteRepository
                    .AddAsync(vote, cancellationToken);



                profile!.ReputationScore += 5;
            }

            _profileRepository.Update(profile);

            await _voteRepository
                .SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}