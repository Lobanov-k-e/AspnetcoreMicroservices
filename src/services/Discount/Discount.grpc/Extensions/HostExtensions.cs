using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Threading;

namespace Discount.grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host, int retryCount = 0)
        {
            const string SettingKey = "Database:ConnectionString";

            using var scope = host.Services.CreateScope();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();

            try
            {
                logger.LogInformation("Migration started");

                var connectionString = configuration.GetValue<string>(SettingKey);
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                using var command = new NpgsqlCommand()
                {
                    Connection = connection
                };
                //dropping tables generally is not good
                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(id SERIAL PRIMARY KEY, 
                                                            product_name VARCHAR(24) NOT NULL,
                                                            description TEXT,
                                                            amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(product_name, description, amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(product_name, description, amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrated database.");

            }
            catch (NpgsqlException e)
            {
                logger.LogError(e.Message);
                if (retryCount < 50)
                {
                    retryCount++;
                    Thread.Sleep(2000);
                    MigrateDatabase<T>(host, retryCount);
                }                
            }

            return host;
        }
    }
}
