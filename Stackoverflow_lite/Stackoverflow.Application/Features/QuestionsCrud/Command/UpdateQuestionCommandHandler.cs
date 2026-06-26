using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Command
{
    public class UpdateQuestionCommandHandler
     : IRequestHandler<UpdateQuestionCommand, ErrorOr<Success>>
    {
        private readonly IQuestionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisCacheService _cache;
        private readonly IViewTrackingService _viewTrackingService;

        public UpdateQuestionCommandHandler(
            IQuestionRepository repository,
            IHttpContextAccessor httpContextAccessor, IRedisCacheService cache,
            IViewTrackingService viewTrackingService)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _viewTrackingService = viewTrackingService;
        }

        public async Task<ErrorOr<Success>> Handle(
            UpdateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question = await _repository
                .GetByIdAsync(request.QuestionId, cancellationToken);

            if (question is null)
            {
                return Error.NotFound(
                    code: "Question.NotFound",
                    description: "Question not found.");
            }

            var userId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("userid")?
                .Value;

            if (question.UserId != Guid.Parse(userId!))
            {
                return Error.Forbidden(
                    code: "Question.UpdateDenied",
                    description: "You can only update your own question.");
            }

            question.Title = request.Title;
            question.Description = request.Description;
            question.TagName = request.TagName;
            question.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(question, cancellationToken);
            await _cache.RemoveAsync(
                 $"question:{request.QuestionId}");

            await _repository.SaveChangesAsync(cancellationToken);


            return Result.Success;
        }
    }
}
