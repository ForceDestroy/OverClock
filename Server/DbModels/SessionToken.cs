using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class SessionToken
    {
        [Key]
        [StringLength(64)]
        [Required]
        public string Token { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
