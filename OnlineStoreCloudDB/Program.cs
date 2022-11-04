using Infrastructure;
using Infrastructure.IRepositories;
using Infrastructure.IServices;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace OnlineStoreCloudDB
{
    public class Program
    {
        public static async Task Main()
        {
            IHost host = new HostBuilder()
                .ConfigureServices(Configure)
                .Build();

            await host.RunAsync();
        }

        private static void Configure(HostBuilderContext builders, IServiceCollection services)
        {
            services.AddDbContext<DBUtils>(option =>
            {
                option.UseCosmos(
                    Environment.GetEnvironmentVariable(
                        "ConnectionString",
                        EnvironmentVariableTarget.Process),
                    Environment.GetEnvironmentVariable("DbName", EnvironmentVariableTarget.Process)
                    );
            });

            services.AddTransient(typeof(IOnlineStoreRead<>),typeof(OnlineStoreRead<>));
            services.AddTransient(typeof(IOnlineStoreWrite<>),typeof(OnlineStoreWrite<>));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IReviewService, ReviewService>();
        }
    }
}
