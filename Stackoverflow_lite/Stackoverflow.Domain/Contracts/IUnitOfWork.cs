using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Domain.Contracts
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
    }
}
