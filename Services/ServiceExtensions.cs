using System.Net.Http;
using ApiRequestManager.Interfaces;
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

            services.AddHttpClient<IBasicIgApiService, BasicIgApiService>();


            return services;
        }
    }
}