using Server.BOModels;

namespace Server.Services.Interfaces
{
    public interface IPayslipService
    {
        public Task<bool> CreatePayslip(TimeSheetSummary timeSheet);

        public Task<ICollection<PaySlipInfo>> GetPayslips(string userID);
    }
}
