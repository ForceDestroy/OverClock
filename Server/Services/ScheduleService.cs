using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Enums;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly DatabaseContext _databaseContext;

        public ScheduleService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        async public Task<bool> ModifySchedule(ICollection<ScheduleInfo> scheduleSubmissions, string userId)
        {
            User? user = await _databaseContext.Users.Include(x => x.Schedules).FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return false;
            }

            DateTime startWeek = GetStartOfWeek(scheduleSubmissions.First().Date);
            DateTime endWeek = startWeek.AddDays(6);

            var newSchedules = scheduleSubmissions.ToList();
            var currentSchedules = await _databaseContext.Schedules.Include(x => x.User).Where(x => string.Equals(x.User.Id, userId) && x.Date.Date >= startWeek.Date && x.Date.Date <= endWeek.Date).ToListAsync();

            _databaseContext.RemoveRange(currentSchedules);

            foreach (var schedule in newSchedules)
            {
                schedule.StartTime = schedule.StartTime.ToLocalTime();
                schedule.EndTime = schedule.EndTime.ToLocalTime();
                Schedule s = new(schedule)
                {
                    User = user
                };
                await _databaseContext.Schedules.AddAsync(s);
            }

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<ScheduleInfo>> GetAllSchedulesForEmployee(string employeeId)
        {
            User? employee = await _databaseContext.Users.Include(x => x.Schedules).FirstOrDefaultAsync(x => x.Id == employeeId);

            if(employee == null)
            {
                return new List<ScheduleInfo>();
            }

            ICollection<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();

            var schedules = await _databaseContext.Schedules.Include(x => x.User).Where(x => string.Equals(x.User.Id, employee.Id)).ToListAsync();
            foreach (Schedule s in schedules)
            {
                scheduleInfoList.Add(new ScheduleInfo(s));
            }

            return scheduleInfoList.OrderByDescending(x => x.StartTime).ToList();
        }
        public async Task<ICollection<ScheduleInfo>> GetSchedulesForAnEmployee(string employeeId, string employerId, DateTime date)
        {
            var localDate = date.ToLocalTime();
            User? employee = await _databaseContext.Users.Include(x => x.Schedules).Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == employeeId);
            User? employer = await _databaseContext.Users.Include(x => x.Manages).FirstOrDefaultAsync(x => x.Id == employerId);

            if (employee == null || employer == null)
            {
                return new List<ScheduleInfo>();
            }

            if (employee.Manager != employer)
            {
                return new List<ScheduleInfo>();
            }

            ICollection<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();

            DateTime startWeek = GetStartOfWeek(localDate);
            DateTime endWeek = GetStartOfWeek(localDate).AddDays(6);

            var schedules = await _databaseContext.Schedules.Include(x => x.User).Where(x => string.Equals(x.User.Id, employee.Id)).ToListAsync();
            foreach (Schedule s in schedules)
            {
                if (DateTime.Compare(startWeek, s.Date) <= 0 && DateTime.Compare(s.Date, endWeek) <= 0)
                {
                    scheduleInfoList.Add(new ScheduleInfo(s));
                }
            }

            return scheduleInfoList.OrderByDescending(x => x.Date).ToList();
        }

        #region helpers
        private static DateTime GetStartOfWeek(DateTime day)
        {
            return day.Date.AddDays(-1 * (int)day.DayOfWeek);
        }
        #endregion

    }
}
