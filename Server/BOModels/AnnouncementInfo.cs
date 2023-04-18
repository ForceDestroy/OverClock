using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DbModels;

namespace Server.BOModels
{
    public class AnnouncementInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public string PosterName { get; set; } = string.Empty;

        public AnnouncementInfo() { }
        public AnnouncementInfo(Announcement a)
        {
            Id = a.Id;
            PosterName = a.User.Name;
            Date = a.Date;
            Title = a.Title;
            Body = a.Body;
        }
    }
}
