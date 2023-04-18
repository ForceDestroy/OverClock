using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.BOModels
{
    public class ModuleSummary
    {
        public int Id { get; set; }
        public string? EmployerId { get; set; }
        public DateTime Date { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public ICollection<ModuleStatusInfo> Statuses { get; set; } = null!;
        public ModuleSummary(Module module)
        {
            Id = module.Id;
            EmployerId = module.Employer.Id;
            Date = module.Date;
            Title = module.Title;
            Body = module.Body;
            Statuses = new List<ModuleStatusInfo>();
            foreach (ModuleStatus s in module.Statuses)
            {
                Statuses.Add(new ModuleStatusInfo(s));
            }
        }

        public ModuleSummary() {}
    }
}
