using Server.DbModels;

namespace Server.BOModels
{
    public class SessionInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public int AccessLevel { get; set; }
        public string Token { get; set; }

        public SessionInfo(SessionToken token, User user)
        {
            Id = user.Id;
            Name = user.Name;
            AccessLevel = user.AccessLevel;
            Token = token.Token;
        }
    }
}
