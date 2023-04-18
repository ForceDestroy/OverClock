using Server.BOModels;
using Server.Data;
using Server.DbModels;

namespace Server.Services.Interfaces
{
    public interface ISysAdminService
    {
        public User? GetUser(string id);
        public User? UpdateUser(UserInfo updatedUser);
        public bool CreateUser(UserInfo newUser);
        public User? DeleteUser(string id);
        public bool SetManagerEmployeeRelationship(string managerId, string employeeId);
        public bool RemoveManagerEmployeeRelationship(string managerId, string employeeId);
    }
}
