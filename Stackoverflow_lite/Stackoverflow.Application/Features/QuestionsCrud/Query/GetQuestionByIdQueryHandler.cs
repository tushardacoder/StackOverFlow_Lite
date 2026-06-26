using MediatR;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{

    public class GetQuestionByIdQueryHandler
        : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
    {
        private readonly IQuestionRepository _repository;
        private readonly IRedisCacheService _cache;
        private readonly IViewTrackingService _viewTrackingService;

        public GetQuestionByIdQueryHandler(
            IQuestionRepository repository, IRedisCacheService cache,
            IViewTrackingService viewTrackingService)
        {
            _repository = repository;
            _cache = cache;
            _viewTrackingService = viewTrackingService;
        }

        public async Task<QuestionDto> Handle(
            GetQuestionByIdQuery request,
            CancellationToken cancellationToken)
        {

            string cacheKey =
               $"question:{request.QuestionId}";

            var cachedQuestion =
                await _cache.GetAsync<QuestionDto>(cacheKey);

            if (cachedQuestion != null)
            {
                await _viewTrackingService
                    .IncrementQuestionViewAsync(
                        request.QuestionId);

                return cachedQuestion;
            }

            var question = await _repository
                .GetByIdAsync(request.QuestionId, cancellationToken);

            if (question == null)
                return null;

            // 3. Map entity to DTO
            var dto = new QuestionDto
            {
                QuestionId = question.QuestionId,
                Title = question.Title,
                Description = question.Description,
                TagName = question.TagName,
                AcceptedAnswer = question.AcceptedAnswer,
                CreatedAt = question.CreatedAt
            };

            // 4. Store in Redis
            await _cache.SetAsync(
                cacheKey,
                dto,
                TimeSpan.FromMinutes(10));

            // 5. Increment view count
            await _viewTrackingService
                .IncrementQuestionViewAsync(
                    request.QuestionId);

            // 6. Return DTO
            return dto;
        }


    }
}