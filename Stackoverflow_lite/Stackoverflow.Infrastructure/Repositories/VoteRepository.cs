using Microsoft.EntityFrameworkCore;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public VoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vote?> GetQuestionVoteAsync(
            Guid userId,
            Guid questionId,
            CancellationToken cancellationToken)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(
                    x => x.UserId == userId &&
                         x.QuestionId == questionId,
                    cancellationToken);
        }

        public async Task<Vote?> GetAnswerVoteAsync(
            Guid userId,
            Guid answerId,
            CancellationToken cancellationToken)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(
                    x => x.UserId == userId &&
                         x.AnswerId == answerId,
                    cancellationToken);
        }

        public async Task AddAsync(
            Vote vote,
            CancellationToken cancellationToken)
        {
            await _context.Votes
                .AddAsync(vote, cancellationToken);
        }

        public Task UpdateAsync(
            Vote vote,
            CancellationToken cancellationToken)
        {
            _context.Votes.Update(vote);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
