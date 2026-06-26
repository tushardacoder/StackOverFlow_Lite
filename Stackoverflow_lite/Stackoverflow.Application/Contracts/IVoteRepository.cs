using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Contracts
{
    public interface IVoteRepository
    {
        Task<Vote?> GetQuestionVoteAsync(
            Guid userId,
            Guid questionId,
            CancellationToken cancellationToken);

        Task<Vote?> GetAnswerVoteAsync(
            Guid userId,
            Guid answerId,
            CancellationToken cancellationToken);

        Task AddAsync(
            Vote vote,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            Vote vote,
            CancellationToken cancellationToken);

        Task SaveChangesAsync(
            CancellationToken cancellationToken);
    }
}
