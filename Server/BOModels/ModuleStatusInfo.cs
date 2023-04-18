using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.Enums;

namespace Server.BOModels
{
    public class ModuleStatusInfo
    {
        public int Id { get; set; }

        public string? EmployeeName { get; set; }

        public string? EmployeeId { get; set; }

        public int? ModuleId { get; set; }

        public string? Status { get; set; }

        public DateTime Date { get; set; }

        public ModuleStatusInfo() { }

        public ModuleStatusInfo(ModuleStatus status)
        {
            Id = status.Id;
            EmployeeName = status.Employee.Name;
            EmployeeId = status.Employee.Id;
            ModuleId = status.Module.Id;
            Status = status.Status;
            Date = status.Date;
        }
    }
}
