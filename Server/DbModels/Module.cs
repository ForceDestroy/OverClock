using Server.BOModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Module
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User Employer { get; set; } = null!;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [StringLength(50)]
        public string? Title { get; set; }

        [Column(TypeName = "text")]
        public string? Body { get; set; }

        public ICollection<ModuleStatus> Statuses { get; set; } = null!;

        public Module(ModuleSummary moduleSummary)
        {
            Id = moduleSummary.Id;
            Date = moduleSummary.Date;
            Title = moduleSummary.Title;
            Body = moduleSummary.Body;
        }

        public Module(ModuleInfo moduleInfo)
        {
            Id = moduleInfo.Id;
            Date = moduleInfo.Date;
            Title = moduleInfo.Title;
            Body = moduleInfo.Body;
        }

        public Module() { }
    }
}
