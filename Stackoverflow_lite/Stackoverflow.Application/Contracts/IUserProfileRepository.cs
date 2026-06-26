using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stackoverflow.Application.Contracts
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(
     Guid userId,
     CancellationToken cancellationToken);

        void Update(UserProfile profile);
    }
}
