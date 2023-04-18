using Server.BOModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class ModuleStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User Employee { get; set; } = null!;

        [ForeignKey("ModuleId")]
        public Module Module { get; set; } = null!;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public ModuleStatus() { }

        public ModuleStatus(ModuleStatusInfo status)
        {
            Id = status.Id;
            Status = status.Status;
            Date = status.Date;
        }
    }
}
