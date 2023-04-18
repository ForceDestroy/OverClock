using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DbModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class WorkHoursInfo
    {
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Function { get; set; } = string.Empty!;
        public string Status { get; set; } = string.Empty!;

        public WorkHoursInfo() { }
        public WorkHoursInfo(WorkHours w)
        {
            Date = w.Date;
            StartTime = w.StartTime;
            EndTime = w.EndTime;
            Function = w.Function;
            Status = w.Status;
        }
    }
}
