using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Policy
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [StringLength(50)]
        public string Hyperlink { get; set; } = string.Empty;
    }
}
