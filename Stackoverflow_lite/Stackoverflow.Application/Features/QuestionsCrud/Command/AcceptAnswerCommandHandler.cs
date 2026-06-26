using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{


    public class AcceptAnswerCommandHandler
        : IRequestHandler<AcceptAnswerCommand, ErrorOr<Success>>
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AcceptAnswerCommandHandler(
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            IUserProfileRepository profileRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _profileRepository = profileRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<Success>> Handle(
            AcceptAnswerCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Error.Unauthorized(
                    code: "Auth.Unauthorized",
                    description: "User is not authenticated.");
            }


            var answer = await _answerRepository
                .GetByIdAsync(
                    request.AnswerId,
                    cancellationToken);

            if (answer is null)
            {
                return Error.NotFound(
                    code: "Answer.NotFound",
                    description: "Answer not found.");
            }

            var question = await _questionRepository
                .GetByIdAsync(
                    answer.QuestionId,
                    cancellationToken);

            if (question is null)
            {
                return Error.NotFound(
                    code: "Question.NotFound",
                    description: "Question not found.");
            }

            // only question owner can accept
            if (question.UserId != Guid.Parse(userId))
            {
                return Error.Forbidden(
                    code: "Answer.AcceptDenied",
                    description: "Only the question owner can accept an answer.");
            }

            // already accepted
            if (answer.IsAccepted)
            {
                return Error.Conflict(
                    code: "Answer.AlreadyAccepted",
                    description: "This answer is already accepted.");
            }

            // remove previous accepted answer
            var allAnswers = await _answerRepository
                .GetByQuestionIdAsync(
                    question.QuestionId,
                    cancellationToken);


            foreach (var item in allAnswers)
            {
                if (item.IsAccepted)
                {
                    item.IsAccepted = false;

                    var profile_before_accepted = await _profileRepository
                        .GetByUserIdAsync(item.UserId, cancellationToken);

                  
                        if (profile_before_accepted!.ReputationScore >= 15)
                        {
                            profile_before_accepted.ReputationScore -= 15;
                        }
                        else if (profile_before_accepted!.ReputationScore > 0)
                        {
                            profile_before_accepted.ReputationScore = 0;
                        }

                    _profileRepository.Update(profile_before_accepted);

                }
            }

            // accept selected answer
            answer.IsAccepted = true;




            // reputation +15
            var profile = await _profileRepository
                .GetByUserIdAsync(
                    answer.UserId,
                    cancellationToken);

            profile!.ReputationScore += 15;
            _profileRepository.Update(profile);


            await _answerRepository
                .SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
