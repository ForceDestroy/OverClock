using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Server.BOModels;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Body { get; set; } = string.Empty;

        public Announcement() { }
        public Announcement(AnnouncementInfo info, User user)
        {
            User = user;
            Date = info.Date;
            Title = info.Title;
            Body = info.Body;
        }
    }
}
