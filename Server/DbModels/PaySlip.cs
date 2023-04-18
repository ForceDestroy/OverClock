using Server.BOModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Xml.Linq;

namespace Server.DbModels
{
    [ExcludeFromCodeCoverage]
    public class PaySlip
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime IssueDate { get; set; }

        [Column(TypeName = "Money")]
        public decimal GrossAmount { get; set; }

        [Column(TypeName = "decimal (18,2)")]
        public decimal HoursWorked { get; set; }

        [Column(TypeName = "Money")]
        public decimal AmountAccumulated { get; set; } = 0;
        [Column(TypeName = "Money")]
        public decimal NetAccumulated { get; set; } = 0;

        [Column(TypeName = "Money")]
        public decimal EIAccumulated { get; set; } = 0;

        [Column(TypeName = "Money")]
        public decimal FITAccumulated { get; set; } = 0;

        [Column(TypeName = "Money")]
        public decimal QCPITAccumulated { get; set; } = 0;

        [Column(TypeName = "Money")]
        public decimal QPIPAccumulated { get; set; } = 0;

        [Column(TypeName = "Money")]
        public decimal QPPAccumulated { get; set; } = 0;

        [Column(TypeName = "decimal (18,2)")]
        public decimal HoursAccumulated { get; set; } = 0;

        public PaySlip() { }

        public PaySlip(PaySlipInfo p)
        {
            StartDate = p.StartDate;
            EndDate = p.EndDate;
            IssueDate = p.IssueDate;
            HoursWorked = p.HoursWorked;
            GrossAmount = p.GrossAmount;
            AmountAccumulated = p.AmountAccumulated;
            EIAccumulated = p.EIAccumulated;
            FITAccumulated = p.FITAccumulated;
            QCPITAccumulated = p.QCPITAccumulated;
            QPIPAccumulated = p.QPIPAccumulated;
            QPPAccumulated = p.QPPAccumulated;
            HoursAccumulated = p.HoursAccumulated;
            NetAccumulated = p.NetAccumulated;
        }
    }
}
