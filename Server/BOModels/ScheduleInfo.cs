using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DbModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class ScheduleInfo
    {
        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public double AllowedBreakTime { get; set; }

        public ScheduleInfo(Schedule schedule)
        {
            Date = schedule.Date;
            StartTime = schedule.StartTime;
            EndTime = schedule.EndTime;
            AllowedBreakTime = schedule.AllowedBreakTime;
            UserName = schedule.User.Name;
            UserId = schedule.User.Id;
        }

        public ScheduleInfo() { }
    }
}
