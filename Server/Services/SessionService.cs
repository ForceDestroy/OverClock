using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using System;
using System.Security.Cryptography;

namespace Server.Services
{
    public class SessionService : ISessionService
    {
        private readonly DatabaseContext _databaseContext;

        private static readonly RandomNumberGenerator random = RandomNumberGenerator.Create();

        public static string RandomString(int length)
        {
            byte[] data = new byte[length];
            random.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public SessionService(DatabaseContext database)
        {
            this._databaseContext = database;
        }

        public async Task<SessionToken> CreateSession(User user)
        {
            User u = (await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id))!;
            SessionToken? currentSession = await _databaseContext.SessionTokens.FirstOrDefaultAsync(x => x.User.Id == user.Id);

            if (currentSession != null)
            {
                _databaseContext.Remove(currentSession);
                await _databaseContext.SaveChangesAsync();
            }

            SessionToken newSession = new()
            {
                Token = string.Concat(RandomString(16), DateTime.UtcNow.ToString("MMddyyyyhmmtt")),
                User = u,
            };

            _databaseContext.SessionTokens.Add(newSession);
            await _databaseContext.SaveChangesAsync();
            return newSession;
        }

        public async Task<bool> EndSession(string Token)
        {
            SessionToken? SessionToRemove = await _databaseContext.SessionTokens.FirstOrDefaultAsync(x => String.Equals(x.Token, Token));

            if (SessionToRemove != null)
            {
                _databaseContext.Remove(SessionToRemove);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<SessionToken?> GetSession(string Token)
        {
            SessionToken? Session = await _databaseContext.SessionTokens.Include(x => x.User).FirstOrDefaultAsync(x => String.Equals(x.Token, Token));

            return Session;
        }
    }
}
