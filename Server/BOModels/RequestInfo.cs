using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DbModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class RequestInfo
    {
        public int Id { get; set; }
        public string FromId { get; set; } = string.Empty;

        public string ToId { get; set; } = string.Empty;

        public string FromName { get; set; } = string.Empty;

        public string ToName { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Type { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public RequestInfo() { }

        public RequestInfo(Request request)
        {
            Id = request.Id;
            FromId = request.FromId;
            ToId = request.ToId;
            FromName = request.From.Name;
            ToName = request.To.Name;
            Body = request.Body;
            Date = request.Date;
            StartTime = request.StartTime;
            EndTime = request.EndTime;
            Type = request.Type;
            Status = request.Status;
        }
    }
}
