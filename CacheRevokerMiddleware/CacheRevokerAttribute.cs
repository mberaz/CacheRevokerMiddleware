using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheRevokerMiddleware;

public class CacheRevokerAttribute : ActionFilterAttribute
{
    public string  Domain { get; set; }

    public string?  KeyParam { get; set; }

    public CacheRevokerAttribute(string domain)
    {
        Domain = domain ?? throw new Exception("The domain cannot be NULL");
    }

    public CacheRevokerAttribute(string domain, string keyParam)
    {
        Domain = domain ?? throw new Exception("The domain cannot be NULL");
        KeyParam = keyParam;
    }
}