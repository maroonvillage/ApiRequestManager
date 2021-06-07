using System.Net.Http;
using ApiRequestManager.Interfaces;
using ApiRequestManager.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRequestManager.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            //services.AddScoped<IMemoryCacheService, CacheService>();
            //services.AddTransient<IBasicIgApiService, BasicIgApiService>();
            // Add all other services here.

            _ = services.AddTransient<IMemoryCacheService, CacheService>((dcx) =>
            {
                IMemoryCache mc = dcx.GetService<IMemoryCache>();

                return new CacheService(mc);

            });

            services.AddTransient<IRequestRepository, RequestRepository>();

            services.AddHttpClient<IBasicIgApiService, BasicIgApiService>();

            services.AddTransient<IRequestService, RequestService>();

            return services;
        }
    }
}