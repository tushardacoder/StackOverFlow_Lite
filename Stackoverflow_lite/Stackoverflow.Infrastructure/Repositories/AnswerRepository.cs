using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Data;

namespace Stackoverflow.Infrastructure.Repositories
{
    

    public class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Answer answer,
            CancellationToken cancellationToken)
        {
            await _context.Answers
                .AddAsync(answer, cancellationToken);
        }

        public async Task<Answer?> GetByIdAsync(
      Guid answerId,
      CancellationToken cancellationToken)
        {
            return await _context.Answers
                .FirstOrDefaultAsync(
                    x => x.AnswerId == answerId,
                    cancellationToken);
        }
        public async Task<List<Answer>> GetByQuestionIdAsync(
      Guid questionId,
      CancellationToken cancellationToken)
        {
            return await _context.Answers
                .AsNoTracking()
                .Where(x => x.QuestionId == questionId)
                .ToListAsync(cancellationToken);
        }
        public Task UpdateAsync(
        Answer answer,
        CancellationToken cancellationToken)
        {
            _context.Answers.Update(answer);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(
       Answer answer,
       CancellationToken cancellationToken)
        {
            _context.Answers.Remove(answer);

            return Task.CompletedTask;
        }

        public async Task<List<Answer>> GetByUserIdAsync(
    Guid userId,
    CancellationToken cancellationToken)
        {
            return await _context.Answers
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }


        public async Task DeleteVotesByAnswerIdAsync(
       Guid answerId,
       CancellationToken cancellationToken)
        {
            var votes = await _context.Votes
                .Where(v => v.AnswerId == answerId)
                .ToListAsync(cancellationToken);

            _context.Votes.RemoveRange(votes);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
