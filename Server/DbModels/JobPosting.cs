using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class JobPosting
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; } = null!;

        [StringLength(50)]
        public string Author { get; set; } = null!;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "Money")]
        public decimal Salary { get; set; }

        [Column(TypeName ="text")]
        public string Description { get; set; } = null!;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<Application> Applications { get; set; } = null!;
    }
}
