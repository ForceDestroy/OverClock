using Server.BOModels;
using Server.Data;

namespace Server.Services.Interfaces
{
    public interface ITimeSheetService
    {
        public Task<bool> LogWorkHours(string userID,IEnumerable<WorkHoursInfo> hours);

        public Task<IEnumerable<WorkHoursInfo>> GetWorkHours(string userID, DateTime day);

        public Task<bool> CheckDaysSubmission(string userID, DateTime day);

        public bool ValidateWorkHours(IEnumerable<WorkHoursInfo> hours);

        public Task<IEnumerable<IEnumerable<WorkHoursInfo>>> GetTimeSheet(string userID, DateTime date);

        public Task SubmitTimeSheet(string userID, IEnumerable<WorkHoursInfo> submission);

        public Task<IEnumerable<TimeSheetSummary>> GetSubmittedTimeSheets(string userID);

        public Task<bool> ApproveTimeSheet(string userID, DateTime date);

        public Task<bool> CheckForApprovedTimeSheet(string userID, DateTime date);
    }
}
