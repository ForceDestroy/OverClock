using Server.Data;
using Server.DbModels;
using Server.BOModels;

namespace Server.Services.Interfaces
{
    public interface IEmployerService
    {
        public Task<UserInfo?> GetEmployee(string userId);

        public Task<ICollection<UserInfo>?> GetListOfEmployees(string employerId);

        public Task<UserInfo?> UpdateEmployee(UserInfo updatedEmployee);

        public Task<bool> CreateEmployee(UserInfo newEmployeeInfo, string employerId);

        public Task<UserInfo?> DeleteEmployee(string userId);

        public Task<bool> ValidateAuthorization(string employerId, string employeeId);
    }
}