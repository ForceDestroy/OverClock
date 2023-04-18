using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Server.BOModels;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

        public double AllowedBreakTime { get; set; }

        public Schedule(ScheduleInfo scheduleInfo)
        {
            Date = scheduleInfo.Date;
            StartTime = scheduleInfo.StartTime;
            EndTime = scheduleInfo.EndTime;
            AllowedBreakTime = scheduleInfo.AllowedBreakTime;
        }
        public Schedule() { }
    }
}
