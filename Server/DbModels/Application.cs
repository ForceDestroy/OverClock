using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "char")]
        public string Referral { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Education { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Experience { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string SkillSet { get; set; } = string.Empty;

        [StringLength(10)]
        public string UserId { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        public int PostingId { get; set; }
        public JobPosting Posting { get; set; } = null!;
    }
}
