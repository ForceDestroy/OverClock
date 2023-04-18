using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.Enums;

namespace Server.BOModels
{
    public class ModuleInfo
    {
        public int Id { get; set; }

        public string? EmployerName { get; set; }

        public string? EmployerId { get; set; }

        public DateTime Date { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }
        public ModuleStatusInfo? Status { get; set; }
        public ModuleInfo(Module module)
        {
            Id = module.Id;
            EmployerName = module.Employer.Name;
            EmployerId = module.Employer.Id;
            Date = module.Date;
            Title = module.Title;
            Body = module.Body;
        }

        public ModuleInfo() { }
    }
}
