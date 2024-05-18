namespace CacheRevokerMiddleware
{
    public interface ICache
    {
        Task Revoke(string domain, string? key);
        Task RevokeDomain(string domain);
    }

    public class MyCache : ICache
    {
        public Task Revoke(string domain, string? key)
        {
            if (key == null)
            {
                return RevokeDomain(domain);
            }
            return Task.CompletedTask;
        }

        public Task RevokeDomain(string domain)
        {
            return Task.CompletedTask;
        }
    }
}
