using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class PayslipService : IPayslipService
    {
        private readonly DatabaseContext _databaseContext;

        public PayslipService(DatabaseContext database)
        {
            this._databaseContext = database;
        }
        public async Task<bool> CreatePayslip(TimeSheetSummary timeSheet)
        {
            User? user = await _databaseContext.Users.Include(x => x.Manages).FirstOrDefaultAsync(x => string.Equals(x.Id, timeSheet.UserId));

            if(user == null)
                return false;

            PaySlip? previousPaySlip = await _databaseContext.Payslips.Where(x => string.Equals(x.User.Id,user.Id)).OrderByDescending(x => x.AmountAccumulated).FirstOrDefaultAsync();

            PaySlipInfo paySlipInfo = new(user, timeSheet);

            if (previousPaySlip != null)
            {
                PaySlipInfo previousPay = new(previousPaySlip);
                paySlipInfo.AmountAccumulated += previousPay.AmountAccumulated;
                paySlipInfo.HoursAccumulated += previousPay.HoursAccumulated;
                paySlipInfo.EIAccumulated += previousPay.EIAccumulated;
                paySlipInfo.FITAccumulated += previousPay.FITAccumulated;
                paySlipInfo.QCPITAccumulated += previousPay.QCPITAccumulated;
                paySlipInfo.QPIPAccumulated += previousPay.QPIPAccumulated;
                paySlipInfo.QPPAccumulated += previousPay.QPPAccumulated;
                paySlipInfo.NetAccumulated += previousPay.NetAccumulated;
            }

            PaySlip paySlip = new(paySlipInfo)
            {
                User = user
            };

            await _databaseContext.AddAsync(paySlip);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<PaySlipInfo>> GetPayslips(string userID)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, userID));

            if (user == null)
                return new List<PaySlipInfo>();

            List<PaySlipInfo> payslipInfos = new();

            var paySlips = await _databaseContext.Payslips.Include(x => x.User).Where(x => string.Equals(x.User.Id, userID)).ToListAsync();

            foreach (PaySlip p in paySlips)
            {
                payslipInfos.Add(new PaySlipInfo(p));
            }

            return payslipInfos;
        }
    }
}
