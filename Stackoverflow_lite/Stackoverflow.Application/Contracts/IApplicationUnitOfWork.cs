using Stackoverflow.Domain.Contracts;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Contracts
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
    }
}
