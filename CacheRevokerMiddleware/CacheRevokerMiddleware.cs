using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Nodes;

namespace CacheRevokerMiddleware
{
    public class CacheRevokerMiddleware(RequestDelegate next, ICache cache)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            CacheRevokerAttribute? cacheRevokerAttribute = null;

            var requestBody = await GetRequestBody(context);

            await next(context);

            //GET endpoints should not be making changes
            if (context.Request.Method != HttpMethods.Get)
            {
                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                cacheRevokerAttribute = endpoint?.Metadata.GetMetadata<CacheRevokerAttribute>();
            }

            if (cacheRevokerAttribute != null)
            {
                if (cacheRevokerAttribute.KeyParam == null)
                {
                    await cache.RevokeDomain(cacheRevokerAttribute.Domain);
                }
                else
                {
                    await cache.Revoke(cacheRevokerAttribute.Domain,
                        GetRequestKey(context.Request, cacheRevokerAttribute.KeyParam, requestBody));
                }
            }
        }

        private static async Task<string> GetRequestBody(HttpContext context)
        {
            //enable the body to be read more than once
            //so the model binding can use id
            context.Request.EnableBuffering();

            var requestBody = await ReadAsStringAsync(context.Request);

            //rewind the request to the start
            context.Request.Body.Position = 0;
            return requestBody;
        }

        public static async Task<string> ReadAsStringAsync(HttpRequest request)
        {
            using StreamReader reader = new(request.Body, leaveOpen: true);
            return await reader.ReadToEndAsync();
        }

        private string GetRequestKey(HttpRequest request, string keyParam, string requestBody)
        {
            if (request.RouteValues.TryGetValue(keyParam, out var keyValue))
            {
                return keyValue.ToString();
            }

            if (request.Query.TryGetValue(keyParam, out var qsKeyValue))
            {
                return qsKeyValue.ToString();
            }

            if (request.Body.CanRead)
            {
                var bodyObject = JsonNode.Parse(requestBody);
                return bodyObject[keyParam]?.AsValue().ToString();
            }

            return null;
        }
    }
}
