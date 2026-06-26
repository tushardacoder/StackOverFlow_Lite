using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Stackoverflow.Application.Contracts;
    using Stackoverflow.Domain.Entities;
    using Stackoverflow.Infrastructure.Data;

    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Question question,
            CancellationToken cancellationToken)
        {
            await _context.Questions
                .AddAsync(question, cancellationToken);


        }

        public async Task<List<Question>> GetAllAsync(
     CancellationToken cancellationToken)
        {
            return await _context.Questions
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Question?> GetByIdAsync(
       Guid id,
       CancellationToken cancellationToken)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(
                    x => x.QuestionId == id,
                    cancellationToken);
        }

        public async Task<List<Question>> GetByTagAsync(
    string tagName,
    CancellationToken cancellationToken)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(x => x.TagName.Contains(tagName))
                .ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(
     Question question,
     CancellationToken cancellationToken)
        {
            _context.Questions.Update(question);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(
            Question question,
            CancellationToken cancellationToken)
        {
            _context.Questions.Remove(question);

            return Task.CompletedTask;
        }

        public async Task<List<Question>> GetQuestionsExceptUserAsync(
 Guid userId,
 CancellationToken cancellationToken)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(x => x.UserId != userId)
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteQuestionVotesAsync(
    Guid questionId,
    CancellationToken cancellationToken)
        {
            var votes = await _context.Votes
                .Where(v => v.QuestionId == questionId)
                .ToListAsync(cancellationToken);

            _context.Votes.RemoveRange(votes);
        }

        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(
    Guid questionId,
    CancellationToken cancellationToken)
        {
            return await _context.Answers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteAnswerVotesAsync(
    Guid answerId,
    CancellationToken cancellationToken)
        {
            var votes = await _context.Votes
                .Where(v => v.AnswerId == answerId)
                .ToListAsync(cancellationToken);

            _context.Votes.RemoveRange(votes);
        }

        public async Task DeleteAnswersByQuestionIdAsync(
    Guid questionId,
    CancellationToken cancellationToken)
        {
            var answers = await _context.Answers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync(cancellationToken);

            _context.Answers.RemoveRange(answers);
        }

        public async Task SaveChangesAsync(
     CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }



    }
}
