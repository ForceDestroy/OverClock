using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class Email
    {
        [Key]
        public int Id { get; set; }

        [StringLength(10)]
        public string FromId { get; set; } = string.Empty;

        [StringLength(10)]
        public string ToId { get; set; } = string.Empty;

        [Required]
        public User? From { get; set; }

        [Required]
        public User To { get; set; } = null!;

        [StringLength(50)]
        public string Subject { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Body { get; set; } = string.Empty;
    }
}