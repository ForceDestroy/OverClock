using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Enums;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class ModuleService : IModuleService
    {
        private readonly DatabaseContext _databaseContext;

        public ModuleService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> CreateModule(ModuleInfo moduleInfo)
        {
            User? employer = await _databaseContext.Users.Include(x => x.Manages).Include(x => x.Modules).FirstOrDefaultAsync(x => x.Id == moduleInfo.EmployerId);

            if (employer == null)
            {
                return false;
            }

            moduleInfo.Date = DateTime.Now;

            var employees = _databaseContext.Users.Where(x => x.Manager.Id == employer.Id);
            Module module = new(moduleInfo)
            {
                Employer = employer,
                Statuses = new List<ModuleStatus>()
            };

            foreach (User user in employees)
            {
                ModuleStatus status = new()
                {
                    Module = module,
                    Employee = user,
                    Status = nameof(TrainingModuleStatus.Unseen)
                };
                await _databaseContext.AddAsync(status);
            }

            await _databaseContext.AddAsync(module);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteModule(int moduleId)
        {
            Module? module = await _databaseContext.Modules.Include(x => x.Statuses).Include(x => x.Employer).FirstOrDefaultAsync(x => x.Id == moduleId);

            if(module == null)
            {
                return false;
            }
            _databaseContext.ModuleStatus.RemoveRange(module.Statuses);
            _databaseContext.Modules.Remove(module);

            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<ModuleSummary>> GetAllModulesForEmployer(string employerId)
        {
            User? employer = await _databaseContext.Users.Include(x => x.Modules).FirstOrDefaultAsync(x => x.Id == employerId);

            if (employer == null)
            {
                return new List<ModuleSummary>();
            }

            if (employer.Modules == null)
            {
                employer.Modules = new List<Module>();
            }

            ICollection<ModuleSummary> listOfModules = new List<ModuleSummary>();
            ICollection<Module> modules = await _databaseContext.Modules.Include(x => x.Statuses).Include(x => x.Employer).Include(x => x.Employer.Manages).Where(x => x.Employer.Id == employerId).ToListAsync();
            foreach (Module m in modules)
            {
                listOfModules.Add(new ModuleSummary(m));
            }

            return listOfModules;
        }

        public async Task<ICollection<ModuleInfo>> GetAllModulesForAnEmployee(string employeeId)
        {
            User? employee = await _databaseContext.Users.Include(x => x.Modules).Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null || employee.Manager == null)
            {
                return new List<ModuleInfo>();
            }

            User? employer = await _databaseContext.Users.Include(x => x.Modules).FirstOrDefaultAsync(x => x.Id == employee.Manager.Id);
            if (employer == null)
            {
                return new List<ModuleInfo>();
            }

            if (employer.Modules == null)
            {
                employer.Modules = new List<Module>();
            }

            ICollection<ModuleInfo> listOfModules = new List<ModuleInfo>();
            ICollection<Module> modules = await _databaseContext.Modules.Include(x => x.Statuses).Include(x => x.Employer).Include(x => x.Employer.Manages).Where(x => x.Employer.Id == employer.Id).ToListAsync();
            foreach (Module m in modules)
            {
                ModuleInfo info = new(m);
                ModuleStatus? status = await _databaseContext.ModuleStatus.Include(x => x.Module).Include(x => x.Employee).FirstOrDefaultAsync(x => x.Module.Id == m.Id && x.Employee.Id == employeeId);

                if (status == null)
                {
                    return new List<ModuleInfo>();
                }
                info.Status = new(status);
                listOfModules.Add(info);
            }

            return listOfModules;
        }

        public async Task<bool> UpdateModuleStatusEmployee(int moduleStatusId, string status)
        {
            ModuleStatus? currentStatus = await _databaseContext.ModuleStatus.Include(x => x.Module).Include(x => x.Employee).FirstOrDefaultAsync(x => x.Id == moduleStatusId);

            if (currentStatus == null)
            {
                return false ;
            }

            if (StringComparison.Equals(status.ToLower(), nameof(TrainingModuleStatus.Unseen).ToLower())) 
            {
                currentStatus.Status = nameof(TrainingModuleStatus.Unseen);
            }
            else if(StringComparison.Equals(status.ToLower(), nameof(TrainingModuleStatus.Pending).ToLower()))
            {
                currentStatus.Status = nameof(TrainingModuleStatus.Pending);
            }
            else if(StringComparison.Equals(status.ToLower(), nameof(TrainingModuleStatus.Completed).ToLower()))
            {
                currentStatus.Status = nameof(TrainingModuleStatus.Completed);
            }
            else
            {
                return false;
            }

            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}
