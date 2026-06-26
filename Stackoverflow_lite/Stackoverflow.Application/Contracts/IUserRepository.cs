using Stackoverflow.Domain.Contracts;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Stackoverflow.Application.Contracts
{
    public interface IUserRepository
    {
        Task AddAsync(
        UserProfile profile,
        CancellationToken cancellationToken);

        

    }
}
