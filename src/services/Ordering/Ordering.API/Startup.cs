using EventBus.Messages.Preferances;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.API.EventConsumers;
using Ordering.Application;
using Ordering.Infrastructure;
using System.Reflection;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.Api", Version = "v1" }));

            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            RegisterEventHandlers(services);

            AddEventbus(services, Configuration);
        }

        private static void RegisterEventHandlers(IServiceCollection services)
        {
            services.AddScoped<BasketCheckoutConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddEventbus(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(conf =>
            {
                conf.AddConsumer<BasketCheckoutConsumer>();

                conf.UsingRabbitMq((ctx, cfg) =>
                {                 
                    cfg.Host(configuration.GetValue<string>("EventBus:Host"));
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
                        c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                    });

                });
            });

            services.AddMassTransitHostedService();
        }

    }
}
