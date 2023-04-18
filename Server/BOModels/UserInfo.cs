using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DbModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int AccessLevel { get; set; }
        public string Address { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int VacationDays { get; set; }

        public int SickDays { get; set; }

        public DateTime Birthday { get; set; }

        public long PhoneNumber { get; set; }

        public int ThemeColor { get; set; }

        public string BankingNumber { get; set; } = string.Empty;

        public string SIN { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public UserInfo(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
            AccessLevel = user.AccessLevel;
            Address = user.Address;
            Salary = user.Salary;
            VacationDays = user.VacationDays;
            SickDays = user.SickDays;
            Birthday = user.Birthday;
            PhoneNumber = user.PhoneNumber;
            ThemeColor = user.ThemeColor;
            BankingNumber = user.BankingNumber;
            SIN = user.SIN;
            Position = user.Position;
        }

        public UserInfo() { }
    }
}
