using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CacheRevokerMiddleware
{
    public static class ServiceCollectionExtensions
    {
        public static void UseCacheRevoker(this IApplicationBuilder app)
        {
            app.UseMiddleware<CacheRevokerMiddleware>();
        }

        ////public static void AddOutputCaching(this IServiceCollection services, Action<OutputCacheOptions> outputCacheOptions)
        ////{
        ////    var options = new OutputCacheOptions();
        ////    outputCacheOptions(options);

        ////    services.AddOutputCaching(options);
        ////}

        public static void AddCacheRevokerServices(this IServiceCollection services)
        {
            services.TryAddSingleton<ICache, MyCache>();
        }
    }
}
