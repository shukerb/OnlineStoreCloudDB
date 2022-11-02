using Infrastructure;
using Infrastructure.IRepositories;
using Infrastructure.IServices;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace OnlineStoreCloudDB
{
    public class OnlineStore
    {

        public static async Task Main()
        {
            IHost host = new HostBuilder()
                .ConfigureAppConfiguration(configureDelegate =>
                {
                    configureDelegate
                    .AddJsonFile("local.settings.json", false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
                })
                .ConfigureServices(Configure)
                .ConfigureOpenApi()
                .Build();

            await host.RunAsync();
        }

        private static void Configure(HostBuilderContext builders, IServiceCollection services)
        {
            services.AddDbContext<DBUtils>(option =>
            {
                option.UseCosmos(
                    Environment.GetEnvironmentVariable("connectionString", EnvironmentVariableTarget.Process),
                    //Environment.GetEnvironmentVariable("CosmosDb:Key", EnvironmentVariableTarget.Process),
                    Environment.GetEnvironmentVariable("DatabaseName", EnvironmentVariableTarget.Process)
                    );
            });

            services.AddTransient(typeof(IOnlineStore<>),typeof(OnlineStore<>));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IReviewService, ReviewService>();
        }
    }
}
