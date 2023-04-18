using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class LoginService : ILoginService
    {
        private readonly DatabaseContext _databaseContext;

        public LoginService(DatabaseContext database)
        {
            this._databaseContext = database;
        }
        public async Task<User?> GetUser(string email)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public async Task<bool> ValidateLogin(string email, string password)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            return user != null && string.Equals(user.Password, EncryptionHelper.EncryptString(password));
        }
    }
}
