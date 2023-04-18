using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.BOModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class WorkHours
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

        [StringLength(20)]
        public string Function { get; set; } = null!;

        [StringLength(20)]
        public string Status { get; set; } = null!;

        public WorkHours() { }
        public WorkHours(WorkHoursInfo w) {
            Date = w.Date;
            StartTime = w.StartTime;
            EndTime = w.EndTime;
            Function = w.Function;
            Status = w.Status;
        }
    }
}
