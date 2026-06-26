using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Contracts
{
    public interface IQuestionRepository
    {
        Task AddAsync(
     Question question,
     CancellationToken cancellationToken);

        Task<List<Question>> GetAllAsync(
       CancellationToken cancellationToken);

        Task<Question?> GetByIdAsync(
       Guid id,
       CancellationToken cancellationToken);

        Task<List<Question>> GetByTagAsync(
    string tagName,
    CancellationToken cancellationToken);

        Task UpdateAsync(
       Question question,
       CancellationToken cancellationToken);

        Task DeleteAsync(
            Question question,
            CancellationToken cancellationToken);

        Task<List<Question>> GetQuestionsExceptUserAsync(
 Guid userId,
 CancellationToken cancellationToken);


        Task DeleteQuestionVotesAsync(
    Guid questionId,
    CancellationToken cancellationToken);


        Task<List<Answer>> GetAnswersByQuestionIdAsync(
        Guid questionId,
        CancellationToken cancellationToken);

        Task DeleteAnswerVotesAsync(
    Guid answerId,
    CancellationToken cancellationToken);

        Task DeleteAnswersByQuestionIdAsync(
    Guid questionId,
    CancellationToken cancellationToken);


        Task SaveChangesAsync(
        CancellationToken cancellationToken);
    }
}








