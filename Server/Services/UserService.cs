using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _databaseContext;

        public UserService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<UserProfile?> UpdateAccount(UserProfile newUser)
        {
            User? oldUser = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == newUser.Id);

            User? oldEmployeeEmail = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == newUser.Email && x.Id != newUser.Id);

            if (oldUser == null || oldEmployeeEmail != null)
            {
                return null;
            }
            else
            {
                if (newUser.Password != oldUser.Password)
                {
                    newUser.Password = EncryptionHelper.EncryptString(newUser.Password);
                }

                _databaseContext.Entry(oldUser).CurrentValues.SetValues(newUser);
                await _databaseContext.SaveChangesAsync();

                UserProfile updatedUserProfile = new(oldUser);
                return updatedUserProfile;
            }
        }

        public async Task<UserProfile?> UpdateTheme(string id, int themeColor)
        {
            User? oldUser = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (oldUser == null)
            {
                return null;
            }
            else
            {
                oldUser.ThemeColor = themeColor;
                await _databaseContext.SaveChangesAsync();

                UserProfile updatedUserProfile = new(oldUser);
                return updatedUserProfile;
            }
        }

        public async Task<UserProfile?> GetAccountInfo(string userId)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }
            else
            {
                UserProfile userInfo = new(user);
                return userInfo;
            }
        }
    }
}
