using Server.BOModels;
using Server.DbModels;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfile?> UpdateAccount(UserProfile newUser);
        public Task<UserProfile?> UpdateTheme(string id, int themeColor);
        public Task<UserProfile?> GetAccountInfo(string userId);
    }
}
