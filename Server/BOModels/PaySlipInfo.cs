using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class PaySlipInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HoursAccumulated { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal AmountAccumulated { get; set; }

        public decimal EI => GrossAmount * 0.012m;
        public decimal FIT => GrossAmount * 0.0668m;
        public decimal QCPIT => GrossAmount * 0.1011m;
        public decimal QPIP => GrossAmount * 0.0049m;
        public decimal QPP => GrossAmount * 0.0572m;

        public decimal EIAccumulated { get; set; }
        public decimal FITAccumulated { get; set; }
        public decimal QCPITAccumulated { get; set; }
        public decimal QPIPAccumulated { get; set; }
        public decimal QPPAccumulated { get; set; }

        public decimal NetAmount => GrossAmount - EI - FIT - QCPIT - QPIP - QPP;

        public decimal NetAccumulated { get; set; }

        public PaySlipInfo() { }
        public PaySlipInfo(User u,TimeSheetSummary t)
        {
            Name = u.Name;
            Address = u.Address;
            Salary = u.Salary;
            StartDate = t.Date;
            EndDate = StartDate.AddDays(6);
            IssueDate = DateTime.Now.ToLocalTime().Date;
            HoursWorked = (decimal)(t.HoursWorked + t.OvertimeWorked + t.DoubleOvertimeWorked);
            GrossAmount = ((decimal)(t.HoursWorked + t.PaidHoliday + t.PaidSick) * Salary) + ((decimal)t.OvertimeWorked * Salary * 1.5m) + ((decimal)t.DoubleOvertimeWorked * Salary * 2);

            AmountAccumulated = GrossAmount;
            HoursAccumulated = HoursWorked;
            EIAccumulated = EI;
            FITAccumulated = FIT;
            QCPITAccumulated = QCPIT;
            QPIPAccumulated = QPIP;
            QPPAccumulated = QPP;
            NetAccumulated = NetAmount;
        }

        public PaySlipInfo(PaySlip p)
        {
            Name = p.User.Name;
            Salary = p.User.Salary;
            Address = p.User.Address;
            StartDate = p.StartDate;
            EndDate = p.EndDate;
            IssueDate = p.IssueDate;
            HoursWorked = p.HoursWorked;
            GrossAmount = p.GrossAmount;

            AmountAccumulated = p.AmountAccumulated;
            HoursAccumulated = p.HoursAccumulated;
            EIAccumulated = p.EIAccumulated;
            FITAccumulated = p.FITAccumulated;
            QCPITAccumulated = p.QCPITAccumulated;
            QPIPAccumulated = p.QPIPAccumulated;
            QPPAccumulated = p.QPPAccumulated;
            NetAccumulated = p.NetAccumulated;
        }
    }
}
