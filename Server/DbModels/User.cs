using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Server.BOModels;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        public User() { }
        public User(UserInfo userInfo) {
            Id = userInfo.Id;
            Name = userInfo.Name;
            Email = userInfo.Email;
            Password = userInfo.Password;
            AccessLevel = userInfo.AccessLevel;
            Address = userInfo.Address;
            Salary = userInfo.Salary;
            VacationDays = userInfo.VacationDays;
            SickDays = userInfo.SickDays;
            Birthday = userInfo.Birthday;
            PhoneNumber = userInfo.PhoneNumber;
            ThemeColor = userInfo.ThemeColor;
            BankingNumber = userInfo.BankingNumber;
            SIN = userInfo.SIN;
            Position = userInfo.Position;
        }

        public User(UserProfile userProfile)
        {
            Id = userProfile.Id;
            Name = userProfile.Name;
            Email = userProfile.Email;
            Password = userProfile.Password;
            Address = userProfile.Address;
            Birthday = userProfile.Birthday;
            PhoneNumber = userProfile.PhoneNumber;
            ThemeColor= userProfile.ThemeColor;
            VacationDays= userProfile.VacationDays;
            SickDays= userProfile.SickDays;
            Position = userProfile.Position;
        }

        [Key]
        [StringLength(10)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Password {get; set; } = string.Empty;

        public int AccessLevel { get; set; }

        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [Column(TypeName = "Money")]
        public decimal Salary { get; set; }

        public int VacationDays { get; set; }

        public int SickDays { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Birthday { get; set; }

        public long PhoneNumber { get; set; }

        public int ThemeColor { get; set; }

        [StringLength(50)]
        public string BankingNumber { get; set; } = string.Empty;

        public string SIN { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public ICollection<Schedule> Schedules { get; set; } = null!;

        public ICollection<Module> Modules { get; set; } = null!;

        public ICollection<Policy> Policies { get; set; } = null!;

        public ICollection<Email> SentEmails { get; set; } = null!;

        public ICollection<Email> ReceivedEmails { get; set; } = null!;

        public ICollection<PaySlip> Payslips { get; set; } = null!;

        public ICollection<Announcement> Announcements { get; set; } = null!;

        public ICollection<WorkHours> WorkHours { get; set; } = null!;

        public ICollection<JobPosting> JobPostings { get; set; } = null!;

        public ICollection<Application> Applications { get; set; } = null!;

        public ICollection<Request> SentRequests { get; set; } = null!;

        public ICollection<Request> ReceivedRequests { get; set; } = null!;

        [ForeignKey("ManagerID")]
        public User? Manager { get; set; }

        public ICollection<User>? Manages { get; set; }
    }
}
