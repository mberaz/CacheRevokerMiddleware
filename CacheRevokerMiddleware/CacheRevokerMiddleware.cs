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
            using StreamReader reader = new(context.Request.Body, leaveOpen: true);
            var requestBody = await reader.ReadToEndAsync();

            //rewind the request to the start
            context.Request.Body.Position = 0;
            return requestBody;
        }

        private string? GetRequestKey(HttpRequest request, string keyParam, string requestBody)
        {
            //try the key in lower case id
            var key = GetKeyFromRequestBody(keyParam , request, requestBody);
            if (key != null)
            {
                return key;
            }
            //change the first letter to upper Id
            key = GetKeyFromRequestBody(ToCamelCase(keyParam), request, requestBody);
            if (key != null)
            {
                return key;
            }
            //try in all upper case
            return GetKeyFromRequestBody(keyParam.ToUpper(), request, requestBody) ?? null;
        }

        private string? GetKeyFromRequestBody(string keyParam, HttpRequest request, string requestBody)
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

        public static string ToCamelCase(string str)
        {
            return char.ToUpper(str[0]) + str[1..];
        }
    }
}
