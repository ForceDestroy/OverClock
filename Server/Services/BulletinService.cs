using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly DatabaseContext _databaseContext;

        public BulletinService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> CreateAnnouncement(AnnouncementInfo announcement, string posterID)
        {
            User? poster = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, posterID));

            if (poster == null)
            {
                return false;
            }

            Announcement a = new(announcement, poster);

            await _databaseContext.AddAsync(a);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAnnouncement(int ID)
        {
            var announcement = await _databaseContext.Announcements.Include(x => x.User).FirstOrDefaultAsync(x => string.Equals(x.Id, ID));

            if (announcement == null)
            {
                return false;
            }

            _databaseContext.Remove(announcement);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AnnouncementInfo>> GetAllAnnouncements()
        {
            var announcements = await _databaseContext.Announcements.Include(x => x.User).ToListAsync();

            List<AnnouncementInfo> announcementInfos = new();

            foreach (var announcement in announcements)
            {
                announcementInfos.Add(new AnnouncementInfo(announcement));
            }

            return announcementInfos.OrderByDescending(x => x.Date).ToList();
        }

        public async Task<IEnumerable<AnnouncementInfo>> GetMyAnnouncements(string userID)
        {
            User? poster = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, userID));

            if (poster == null)
            {
                return new List<AnnouncementInfo>();
            }

            var announcements = await _databaseContext.Announcements.Include(x => x.User).Where(x => string.Equals(x.User.Id, poster.Id)).ToListAsync();

            List<AnnouncementInfo> announcementInfos = new();

            foreach (var announcement in announcements)
            {
                announcementInfos.Add(new AnnouncementInfo(announcement));
            }

            return announcementInfos.OrderByDescending(x => x.Date).ToList();
        }
    }
}
