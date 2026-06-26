using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stackoverflow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Stackoverflow.Infrastructure.External
{
    public static class DbContextExtension
    {
        public static void AddApplicationDbContext(this IServiceCollection services, string connectionString, Assembly migrationAssembly)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString,
                x => x.MigrationsAssembly(migrationAssembly)));
        }
    }
}
