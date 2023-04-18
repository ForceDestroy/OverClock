using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.BOModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [StringLength(10)]
        public string FromId { get; set; } = string.Empty;

        [StringLength(10)]
        public string ToId { get; set; } = string.Empty;

        [Required]
        public User From { get; set; } = null!;

        [Required]
        public User To { get; set; } = null!;

        [Column(TypeName = "text")]
        public string Body { get; set; } = string.Empty;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "Date")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "Date")]
        public DateTime EndTime { get; set; }

        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        public Request() { }

        public Request(RequestInfo requestInfo)
        {
            Id = requestInfo.Id;
            FromId = requestInfo.FromId;
            ToId = requestInfo.ToId;
            Body = requestInfo.Body;
            Date = requestInfo.Date;
            StartTime = requestInfo.StartTime;
            EndTime = requestInfo.EndTime;
            Type = requestInfo.Type;
            Status = requestInfo.Status;
        }
    }
}
