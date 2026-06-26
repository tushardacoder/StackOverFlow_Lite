using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure
{
    public abstract class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
