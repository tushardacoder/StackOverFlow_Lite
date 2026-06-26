using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Contracts
{
    public interface IViewTrackingService
    {
        Task IncrementQuestionViewAsync(Guid questionId);

        Task<long> GetQuestionViewsAsync(Guid questionId);
    }
}
