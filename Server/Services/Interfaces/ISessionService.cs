using Server.DbModels;

namespace Server.Services.Interfaces
{
    public interface ISessionService
    {
        public Task<bool> EndSession(string Token);

        public Task<SessionToken> CreateSession(User user);

        public Task<SessionToken?> GetSession(string Token);
    }
}
