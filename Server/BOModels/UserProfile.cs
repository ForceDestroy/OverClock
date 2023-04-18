using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public long PhoneNumber { get; set; }

        public int ThemeColor { get; set; }

        public int VacationDays { get; set; }

        public int SickDays { get; set; }

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public UserProfile(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
            Address = user.Address;
            Birthday = user.Birthday;
            PhoneNumber = user.PhoneNumber;
            ThemeColor = user.ThemeColor;
            VacationDays = user.VacationDays;
            SickDays = user.SickDays;
            Position = user.Position;
        }

        public UserProfile() { }
    }
}
