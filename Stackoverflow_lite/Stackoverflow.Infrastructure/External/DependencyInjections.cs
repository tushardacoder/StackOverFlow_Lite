using Microsoft.Extensions.DependencyInjection;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.External
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

          

            return services;
        }
    }
}
