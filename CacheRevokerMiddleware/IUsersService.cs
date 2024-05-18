namespace CacheRevokerMiddleware
{
    public interface IUsersService
    {
        Task Create(UserModel user);
        Task Update(UserModel user);
    }

    public class UsersService : IUsersService
    {
        public Task Create(UserModel user)
        {
           return Task.CompletedTask;
        }

        public Task Update(UserModel user)
        {
            return Task.CompletedTask;
        }
    }
}
