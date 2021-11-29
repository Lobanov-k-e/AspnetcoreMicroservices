using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persisence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mailing;
using Ordering.Infrastructure.Persisence;
using Ordering.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString");

            services.AddDbContext<OrderContext>(opts =>
                opts.UseSqlServer(connectionString));

            services.AddTransient<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(e => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();             

            return services;
        }
    }
}
