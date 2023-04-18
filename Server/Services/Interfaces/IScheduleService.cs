using Server.Data;
using Server.DbModels;
using Server.BOModels;

namespace Server.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<bool> ModifySchedule(ICollection<ScheduleInfo> scheduleSubmissions, string userId);

        public Task<ICollection<ScheduleInfo>> GetAllSchedulesForEmployee(string employeeId);

        public Task<ICollection<ScheduleInfo>> GetSchedulesForAnEmployee(string employeeId, string employerId, DateTime date);
    }
}
