using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Contracts
{
    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiration);

        Task RemoveAsync(string key);
    }
}
