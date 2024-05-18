namespace CacheRevokerMiddleware
{
    public interface ICache
    {
        Task Revoke(string key);
        Task RevokeDomain(string domain);
    }

    public class MyCache : ICache
    {
        public Task Revoke(string key)
        {
            throw new NotImplementedException();
        }

        public Task RevokeDomain(string domain)
        {
            throw new NotImplementedException();
        }
    }
}
