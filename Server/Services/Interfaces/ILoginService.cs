using Server.DbModels;

namespace Server.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<User?> GetUser(string email);
        public Task<bool> ValidateLogin(string email, string password );
    }
}
