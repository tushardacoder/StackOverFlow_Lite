using Stackoverflow.Application.Contracts;
using Stackoverflow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IUserRepository UserRepository { get; private set; }

        public ApplicationUnitOfWork(ApplicationDbContext dbContext, IUserRepository userRepository)
            : base(dbContext)
        {
            UserRepository = userRepository;
        }
    }
}
