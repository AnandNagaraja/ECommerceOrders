using System;
using ECommerceOrders.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECommerceOrders.DAL;
using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using ECommerceOrders.Repositories;
using ECommerceOrders.Services;
using Microsoft.OpenApi.Models;


namespace ECommerceOrders
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

            services.AddDbContext<ReadOnlyECommerceDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetValue<string>("ConnectionStrings:OrderDatabase"));
            });


            services.AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IBaseRepository<Order>, BaseRepository<Order>>()
                .AddSingleton<IConfigurationRepository, ConfigurationRepository>();

            //Todo: add the lifetime and and retryPolicy based on the requirement
            //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<IUserService, UserService>();


            // Register the swagger generator
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ECommerce Orders API",
                    Description = "It is readonly API to get Order details based on customer info"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            // Enable middleware to serve generated swagger endpoint
            app.UseSwagger();

            //Specifying the swagger JSON endpoint
            app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce Orders API");
                }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
