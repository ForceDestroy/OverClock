using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Enums;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly DatabaseContext _databaseContext;

        public TimeSheetService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #region EmployeeMethods
        public async Task<IEnumerable<IEnumerable<WorkHoursInfo>>> GetTimeSheet(string userID, DateTime date)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, userID));

            if (user == null)
                return new List<List<WorkHoursInfo>>();

            DateTime startOfWeek = GetStartOfWeek(date.Date);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var workHours = await _databaseContext.WorkHours.Include(x => x.User).Where(x => string.Equals(x.User.Id, userID) && x.Date.Date >= startOfWeek.Date && x.Date.Date <= endOfWeek.Date).ToListAsync();

            List<List<WorkHoursInfo>> result = new();

            for (DateTime i = startOfWeek; i.Date <= endOfWeek.Date; i = i.AddDays(1))
            {
                List<WorkHoursInfo> day = new();

                foreach (WorkHours w in workHours.Where(x => x.Date.Date == i.Date))
                {
                    WorkHoursInfo info = new(w);
                    day.Add(info);
                }

                result.Add(day);
            }

            return result;
        }

        public async Task<bool> LogWorkHours(string userID, IEnumerable<WorkHoursInfo> hours)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, userID));

            if (user == null)
                return false;

            foreach (var timeBlock in hours)
            {
                WorkHours workHours = new() {
                    User = user,
                    Date = timeBlock.Date,
                    StartTime = timeBlock.StartTime.ToLocalTime(),
                    EndTime = timeBlock.EndTime.ToLocalTime(),
                    Function = nameof(WorkHourFunctions.Work),
                    Status = nameof(WorkHourStatus.Draft)
                };

                await _databaseContext.WorkHours.AddAsync(workHours);
            }

            await _databaseContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<WorkHoursInfo>> GetWorkHours(string userID, DateTime day)
        {
            var localDay = day.ToLocalTime();
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, userID));

            if (user == null)
                return new List<WorkHoursInfo>();

            var hours = await _databaseContext.WorkHours.Include(x => x.User).Where(x => x.User == user && x.Date.Date == localDay.Date && string.Equals(x.Function, nameof(WorkHourFunctions.Work))).ToListAsync();

            List<WorkHoursInfo> result = new();

            foreach (WorkHours w in hours)
            {
                result.Add(new WorkHoursInfo(w));
            }

            return result;
        }

        public async Task SubmitTimeSheet(string userID, IEnumerable<WorkHoursInfo> submission)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Id, userID));

            if (user == null)
                return;

            DateTime startOfWeek = GetStartOfWeek(submission.First().Date);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var newWorkHours = submission.ToList();
            var workHours = await _databaseContext.WorkHours.Include(x => x.User).Where(x => string.Equals(x.User.Id, userID) && x.Date.Date >= startOfWeek.Date && x.Date.Date <= endOfWeek.Date
                && string.Equals(x.Function, nameof(WorkHourFunctions.Work))).ToListAsync();

            var paidHours = await _databaseContext.WorkHours.Include(x => x.User).Where(x => string.Equals(x.User.Id, userID) && x.Date.Date >= startOfWeek.Date && x.Date.Date <= endOfWeek.Date
                && !string.Equals(x.Function, nameof(WorkHourFunctions.Work))).ToListAsync();

            _databaseContext.RemoveRange(workHours);

            foreach (var newWorkHour in newWorkHours)
            {
                newWorkHour.StartTime = newWorkHour.StartTime.ToLocalTime();
                newWorkHour.EndTime = newWorkHour.EndTime.ToLocalTime();

                WorkHours workHour = new(newWorkHour)
                {
                    Status = nameof(WorkHourStatus.Submitted),
                    Function = nameof(WorkHourFunctions.Work),
                    User = user
                };

                await _databaseContext.WorkHours.AddAsync(workHour);
            }

            foreach (var paidHour in paidHours)
            {
                paidHour.Status = nameof(WorkHourStatus.Submitted);
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> CheckForApprovedTimeSheet(string userID, DateTime date)
        {
            DateTime startOfWeek = GetStartOfWeek(date.Date);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var approvedHours = await _databaseContext.WorkHours.Include(x => x.User).Where(x => string.Equals(x.User.Id, userID) && x.Date.Date >= startOfWeek.Date && x.Date.Date <= endOfWeek.Date
                && string.Equals(x.Status, nameof(WorkHourStatus.Approved))).ToListAsync();

            return approvedHours.Count > 0;
        }

        #endregion

        #region Helpers
        public bool ValidateWorkHours(IEnumerable<WorkHoursInfo> hours)
        {
            //Check that all TimeBlocks share the same date
            var day = hours.First().Date.ToLocalTime().Date;

            foreach (var hour in hours)
            {
                if (hour.Date.ToLocalTime().Date != day.ToLocalTime().Date)
                {
                    return false;
                }
            }

            return true;
        }

        private static DateTime GetStartOfWeek(DateTime day) {
            return day.Date.AddDays(-1 * (int)day.DayOfWeek);
        }

        public async Task<bool> CheckDaysSubmission(string userID, DateTime day)
        {
            var submitted = await _databaseContext.WorkHours.Include(x => x.User).FirstOrDefaultAsync(x => string.Equals(x.User.Id, userID) && x.Date.Date == day.Date);

            return submitted != null;
        }

        #endregion

        #region EmployerMethods
        public async Task<IEnumerable<TimeSheetSummary>> GetSubmittedTimeSheets(string userID)
        {
            List<User> managedUsers = await _databaseContext.Users.Include(x => x.WorkHours).Where(x => string.Equals(x.Manager.Id, userID)).ToListAsync();

            List<TimeSheetSummary> timeSheets = new();

            foreach (User u in managedUsers)
            {
                List<WorkHours> submittedHours = await _databaseContext.WorkHours.Where(x => string.Equals(x.User.Id, u.Id) && string.Equals(x.Status, nameof(WorkHourStatus.Submitted))).ToListAsync();
                submittedHours.Sort((a, b) => a.Date.CompareTo(b.Date));

                List<WorkHours> workWeek = new();

                if (submittedHours.Count > 0)
                {
                    DateTime weekStart = GetStartOfWeek(submittedHours[0].Date);

                    foreach (WorkHours submittedHour in submittedHours)
                    {
                        DateTime start = GetStartOfWeek(submittedHour.Date);
                        if (start == weekStart)
                        {
                            workWeek.Add(submittedHour);
                        }
                        else
                        {
                            timeSheets.Add(new TimeSheetSummary(workWeek, u));
                            workWeek.Clear();

                            weekStart = start;
                            workWeek.Add(submittedHour);
                        }
                    }

                    timeSheets.Add(new TimeSheetSummary(workWeek, u));
                    workWeek.Clear();
                }
            }

            return timeSheets;
        }

        public async Task<bool> ApproveTimeSheet(string userID, DateTime date)
        {
            List<WorkHours> submittedHours = await _databaseContext.WorkHours.Where(x => string.Equals(x.User.Id, userID) && string.Equals(x.Status, nameof(WorkHourStatus.Submitted))).ToListAsync();

            submittedHours = submittedHours.Where(x => GetStartOfWeek(x.Date) == date).ToList();

            if (submittedHours.Count == 0)
            {
                return false;
            }

            foreach (WorkHours workHour in submittedHours)
            {
                workHour.Status = nameof(WorkHourStatus.Approved);
            }

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
