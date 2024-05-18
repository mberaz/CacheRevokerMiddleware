using Microsoft.AspNetCore.Http.Features;

namespace CacheRevokerMiddleware
{
    public class CacheRevokerMiddleware
    {
        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

            var outputCacheAttribute = endpoint?.Metadata.GetMetadata<OutputCacheAttribute>();
            await _next(context);
        }
    }
}
