namespace CacheRevokerMiddleware
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateUserModel
    {
        public string Name { get; set; }

        public UserModel ToUserModel(int id)
        {
            return new UserModel
            {
                Id = id,
                Name = Name
            };
        }
    }
}
