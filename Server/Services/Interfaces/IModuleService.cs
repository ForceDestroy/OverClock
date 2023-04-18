using Server.BOModels;
using Server.Enums;

namespace Server.Services.Interfaces
{
    public interface IModuleService
    {
        public Task<bool> CreateModule(ModuleInfo moduleInfo);
        public Task<bool> DeleteModule(int moduleId);
        public Task<ICollection<ModuleSummary>> GetAllModulesForEmployer(string employerId);
        public Task<ICollection<ModuleInfo>> GetAllModulesForAnEmployee(string employeeId);
        public Task<bool> UpdateModuleStatusEmployee(int moduleStatusInfoId, string status);
    }
}
