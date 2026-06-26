using StackExchange.Redis;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.Services
{
    public class RedisViewTrackingService
       : IViewTrackingService
    {
        private readonly IDatabase _database;

        public RedisViewTrackingService(
            IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task IncrementQuestionViewAsync(
            Guid questionId)
        {
            string key = $"question:views:{questionId}";

            await _database.StringIncrementAsync(key);
        }

        public async Task<long> GetQuestionViewsAsync(
            Guid questionId)
        {
            string key = $"question:views:{questionId}";

            var views = await _database.StringGetAsync(key);

            return views.HasValue
                ? (long)views
                : 0;
        }
    }
}
