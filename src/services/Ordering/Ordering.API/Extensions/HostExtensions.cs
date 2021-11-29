using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host,
            Action<T, IServiceProvider> seedStrategy,
            int retryCount = 0)
            where T : DbContext 
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetService<T>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();

            try
            {
                logger.LogInformation($"Migration of context {nameof(T)} started");
                Seed(context, seedStrategy, scope.ServiceProvider);
                logger.LogInformation($"Migration of context {nameof(T)} completed");
            }
            catch(SqlException e) 
            {
                logger.LogError($"Error Migration of context {nameof(T)}: {e.Message}");
                while (retryCount++ < 50)
                {
                    System.Threading.Thread.Sleep(2000);
                    MigrateDatabase(host, seedStrategy, retryCount);
                }
            }

            return host;
        }

        private static void Seed<T>(T context, 
            Action<T, IServiceProvider> seed,
            IServiceProvider serviceProvider) 
            where T : DbContext
        {
            context.Database.Migrate();
            seed(context, serviceProvider);
        }
    }
}
