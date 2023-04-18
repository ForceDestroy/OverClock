using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Enums;

namespace Server.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly DatabaseContext _databaseContext;

        public EmployerService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<UserInfo?> GetEmployee(string userId)
        {
            User? employee = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (employee != null)
            {
                UserInfo userInfo = new(employee);
                userInfo.BankingNumber = EncryptionHelper.DecryptString(userInfo.BankingNumber);
                return userInfo;
            }
            else
            {
                return null;
            }
        }

        public async Task<ICollection<UserInfo>?> GetListOfEmployees(string employerId)
        {
            User? employer = await _databaseContext.Users.Include(x => x.Manages).FirstOrDefaultAsync(x => x.Id == employerId);

            if (employer == null)
            {
                return null;
            }

            if(employer.Manages == null)
            {
                return new List<UserInfo>();
            }

            ICollection<UserInfo> listOfEmployees = new List<UserInfo>();

            var users = await _databaseContext.Users.Include(x => x.Manager).Where(x => x.Manager != null && x.Manager.Id == employerId).ToListAsync();

            foreach (User u in users)
            {
                UserInfo info =   new(u);
                info.BankingNumber = EncryptionHelper.DecryptString(info.BankingNumber);
                listOfEmployees.Add(info);
            }

            if (listOfEmployees.Count == 0)
            {
                return listOfEmployees;
            }

            return listOfEmployees.OrderByDescending(x => x.Id).ToList();
        }

        async public Task<UserInfo?> UpdateEmployee(UserInfo updatedEmployee)
        {
            User? oldEmployee = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == updatedEmployee.Id);

            User? oldEmployeeEmail = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == updatedEmployee.Email && x.Id != updatedEmployee.Id && x.Id != updatedEmployee.Id);

            if (oldEmployee == null || oldEmployeeEmail != null)
            {
                return null;
            }
            else
            {
                updatedEmployee.BankingNumber = EncryptionHelper.EncryptString(updatedEmployee.BankingNumber);
                _databaseContext.Entry(oldEmployee).CurrentValues.SetValues(updatedEmployee);
                await _databaseContext.SaveChangesAsync();

                return new UserInfo(oldEmployee);
            }
        }

        public async Task<bool> CreateEmployee(UserInfo newEmployeeInfo, string employerId)
        {
            User? oldEmployeeEmail = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == newEmployeeInfo.Email);

            User? employer = await _databaseContext.Users.Include(x => x.Modules).FirstOrDefaultAsync(x => string.Equals(x.Id, employerId));

            if (oldEmployeeEmail != null || employer == null)
            {
                return false;
            }
            else
            {
                User newEmployee = new(newEmployeeInfo)
                {
                    Id = GetNextID(employerId)
                };
                newEmployee.BankingNumber = EncryptionHelper.EncryptString(newEmployee.BankingNumber);
                newEmployee.SIN = EncryptionHelper.EncryptString(newEmployee.SIN);
                newEmployee.Password = EncryptionHelper.EncryptString(newEmployee.Password);
                newEmployee.Manager = employer;

                _databaseContext.Add(newEmployee);

                ICollection<Module> modules = await _databaseContext.Modules.Include(x => x.Statuses).Include(x => x.Employer).Include(x => x.Employer.Manages).Where(x => x.Employer.Id == employer.Id).ToListAsync();
                foreach (Module module in modules)
                {
                    ModuleStatus moduleStatus = new()
                    {
                        Date = DateTime.Now,
                        Module = module,
                        Status = nameof(TrainingModuleStatus.Unseen),
                        Employee = newEmployee
                    };
                    module.Statuses.Add(moduleStatus);
                    await _databaseContext.AddAsync(moduleStatus);
                }

                await _databaseContext.SaveChangesAsync();

                return true;
            }
        }

        public async Task<UserInfo?> DeleteEmployee(string userId)
        {
            User? employee = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (employee != null)
            {
                var schedules = _databaseContext.Schedules.Where(x => x.User.Id == employee.Id);
                _databaseContext.Schedules.RemoveRange(schedules);

                var moduleStatus = _databaseContext.ModuleStatus.Where(x => x.Employee.Id == employee.Id);
                _databaseContext.ModuleStatus.RemoveRange(moduleStatus);

                var modules = _databaseContext.Modules.Where(x => x.Employer.Id == employee.Id);
                _databaseContext.Modules.RemoveRange(modules);

                var policies = _databaseContext.Policies.Where(x => x.User.Id == employee.Id);
                _databaseContext.Policies.RemoveRange(policies);

                var emails = _databaseContext.Emails.Where(x => x.FromId == employee.Id || x.ToId == employee.Id);
                _databaseContext.Emails.RemoveRange(emails);

                var payslips = _databaseContext.Payslips.Where(x => x.User.Id == employee.Id);
                _databaseContext.Payslips.RemoveRange(payslips);

                var announcements = _databaseContext.Announcements.Where(x => x.User.Id == employee.Id);
                _databaseContext.Announcements.RemoveRange(announcements);

                var workHours = _databaseContext.WorkHours.Where(x => x.User.Id == employee.Id);
                _databaseContext.WorkHours.RemoveRange(workHours);

                var jobPostings = _databaseContext.JobPostings.Where(x => x.User.Id == employee.Id);
                _databaseContext.JobPostings.RemoveRange(jobPostings);

                var applications = _databaseContext.Applications.Where(x => x.User.Id == employee.Id);
                _databaseContext.Applications.RemoveRange(applications);

                var requests = _databaseContext.Requests.Where(x => x.FromId == employee.Id || x.ToId == employee.Id);
                _databaseContext.Requests.RemoveRange(requests);

                var sessions = _databaseContext.SessionTokens.Where(x => x.User.Id == employee.Id);
                _databaseContext.SessionTokens.RemoveRange(sessions);

                employee.Manager = null;
                employee.Manages = null;
                _databaseContext.Users.Remove(employee);
                await _databaseContext.SaveChangesAsync();

                return new UserInfo(employee);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ValidateAuthorization(string employerId, string employeeId)
        {
            User? employee = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
            {
                return false;
            }

            User? employer = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == employerId);

            if (employer == null)
            {
                return false;
            }

            return employer.AccessLevel > employee.AccessLevel;
        }

        private string GetNextID(string managerID)
        {
            string companyID = managerID[..3];

            var latestUser = _databaseContext.Users.Where(x => string.Equals(x.Id.Substring(0, 3), companyID)).OrderByDescending(x => x.Id).FirstOrDefault();

            int nextID;
            if (latestUser == null)
            {
                nextID = 0;
            }
            else
            {
                nextID = Int32.Parse(latestUser.Id.Substring(4, 6)) + 1;
            }

            return string.Concat(companyID, "_", nextID.ToString("D6"));
        }
    }
}