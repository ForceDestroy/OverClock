using Server.BOModels;

namespace Server.Services.Interfaces
{
    public interface IBulletinService
    {
        public Task<IEnumerable<AnnouncementInfo>> GetAllAnnouncements();

        public Task<IEnumerable<AnnouncementInfo>> GetMyAnnouncements(string userID);

        public Task<bool> DeleteAnnouncement(int ID);

        public Task<bool> CreateAnnouncement(AnnouncementInfo announcement, string posterID);
    }
}
