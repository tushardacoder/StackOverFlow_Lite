using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stackoverflow.Application.Contracts
{
    public interface IAnswerRepository
    {

        Task AddAsync(
        Answer answer,
        CancellationToken cancellationToken);

        Task<Answer?> GetByIdAsync(
        Guid answerId,
        CancellationToken cancellationToken);

        Task UpdateAsync(
     Answer answer,
     CancellationToken cancellationToken);

        Task<List<Answer>> GetByUserIdAsync(
    Guid userId,
    CancellationToken cancellationToken);

        Task<List<Answer>> GetByQuestionIdAsync(
    Guid questionId,
    CancellationToken cancellationToken);

    

        Task DeleteAsync(
       Answer answer,
       CancellationToken cancellationToken);



        Task DeleteVotesByAnswerIdAsync(
            Guid answerId,
            CancellationToken cancellationToken);


        Task SaveChangesAsync(
        CancellationToken cancellationToken);
    }
}
