using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheRevokerMiddleware;

public class OutputCacheAttribute : ActionFilterAttribute
{
    public OutputCacheAttribute()
    {

    }

    public OutputCacheAttribute(int durationInSeconds)
    {
        DurationInSeconds = durationInSeconds;
    }

    public int? DurationInSeconds { get; set; }

    public TimeSpan? DurationInTimeSpan()
    {
        return DurationInSeconds == null ? null : TimeSpan.FromSeconds((double)DurationInSeconds);
    }
}