using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class SysAdminService : ISysAdminService
    {
        private readonly DatabaseContext _databaseContext;

        public SysAdminService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public User? GetUser(string id)
        {
            User? user = _databaseContext.Users.FirstOrDefault(x => x.Id == id);

            return user;
        }

        public User? UpdateUser(UserInfo updatedUser)
        {
            User? oldUser = _databaseContext.Users.FirstOrDefault(x => x.Id == updatedUser.Id);

            User? oldEmployeeEmail = _databaseContext.Users.FirstOrDefault(x => x.Email == updatedUser.Email && x.Id != updatedUser.Id);

            if (oldUser == null || oldEmployeeEmail != null)
            {
                return null;
            }
            else
            {
                updatedUser.Password = EncryptionHelper.EncryptString(updatedUser.Password);
                updatedUser.SIN = EncryptionHelper.EncryptString(updatedUser.SIN);
                updatedUser.BankingNumber = EncryptionHelper.EncryptString(updatedUser.BankingNumber);
                _databaseContext.Entry(oldUser).CurrentValues.SetValues(updatedUser);
                _databaseContext.SaveChanges();

                return oldUser;
            }
        }

        public bool CreateUser(UserInfo newUser)
        {
            User? user = _databaseContext.Users.FirstOrDefault(x => x.Id == newUser.Id);

            User? oldEmployeeEmail = _databaseContext.Users.FirstOrDefault(x => x.Email == newUser.Email);

            if (user != null || oldEmployeeEmail != null)
            {
                return false;
            }
            else
            {
                newUser.Password = EncryptionHelper.EncryptString(newUser.Password);
                newUser.SIN = EncryptionHelper.EncryptString(newUser.SIN);
                newUser.BankingNumber = EncryptionHelper.EncryptString(newUser.BankingNumber);
                _databaseContext.Add(new User(newUser));
                _databaseContext.SaveChanges();

                return true;
            }
        }

        public User? DeleteUser(string id)
        {
            User? user = _databaseContext.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();
                return user;
            }
            else
            {
                return null;
            }
        }

        public bool SetManagerEmployeeRelationship(string managerId, string employeeId)
        {
            User? manager = _databaseContext.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == managerId);
            User? employee = _databaseContext.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employeeId);

            if( manager == null || employee == null || manager.Manages == null)
            {
                return false;
            }

            // Duplicate prevention
            if(manager.Manages.Any(x => x.Id.Equals(employeeId)))
            {
                return false;
            }

            employee.Manager = manager;
            _databaseContext.Update(employee);
            _databaseContext.Update(manager);
            _databaseContext.SaveChanges();

            return true;
        }

        public bool RemoveManagerEmployeeRelationship(string managerId, string employeeId)
        {
            User? manager = _databaseContext.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == managerId);
            User? employee = _databaseContext.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employeeId);

            if (manager == null || employee == null)
            {
                return false;
            }

            employee.Manager = null;
            _databaseContext.Update(employee);
            _databaseContext.Update(manager);
            _databaseContext.SaveChanges();

            return true;
        }
    }
}
