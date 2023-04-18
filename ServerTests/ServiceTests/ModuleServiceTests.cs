using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.BOModels;
using Server.Enums;
using Microsoft.Extensions.Primitives;

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class ModuleServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public ModuleServiceTests()
        {

        }

        #region CreateModule
        [Fact]
        public async void ModuleService_CreateModule_When_ValidBehaviour()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            ModuleInfo module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                EmployerId = employer.Id,
                Date = DateTime.Now
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.CreateModule(module);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.Equal(module.Title, response2.Title);
                Assert.Equal(module.Body, response2.Body);
                Assert.Equal(module.Date, response2.Date);
                Assert.Equal(module.EmployerId, employer.Id);
                Assert.NotEmpty(response2.Statuses);
                Assert.Equal(nameof(TrainingModuleStatus.Unseen), response3.Status);
                Assert.Equal(userId, response3.Employee.Id);
                Assert.Equal(response2.Id, response3.Module.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_CreateModule_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            ModuleInfo module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                EmployerId = employer.Id,
                Date = DateTime.Now
            };


            try
            {
                // Act
                var response = await moduleService.CreateModule(module);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.False(response);
                Assert.Null(response2);
                Assert.Null(response3);
                
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region DeleteModule
        [Fact]
        public async void ModuleService_DeleteModule_When_ModuleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.DeleteModule(module.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.True(response);
                Assert.Null(response2);
                Assert.Null(response3);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_DeleteModule_When_ModuleDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            
            db.Add(employee);
            db.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.DeleteModule(module.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.False(response);
                Assert.Null(response2);
                Assert.Null(response3);
                

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region GetAllModulesForEmployer
        [Fact]
        public async void ModuleService_GetAllModulesForEmployer_When_ModulesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForEmployer(employer.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.NotEmpty(response);
                Assert.Equal(module.Title, response.First().Title);
                Assert.Equal(module.Body, response.First().Body);
                Assert.NotEmpty(response.First().Statuses);
                Assert.Equal(moduleStatus.Status, response.First().Statuses.First().Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_GetAllModulesForEmployer_When_NoModuleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Modules = null
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(employee);
            db.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForEmployer(employer.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.Empty(response);
                Assert.NotNull(employer.Modules);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_GetAllModulesForEmployer_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForEmployer(employer.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Empty(response);
                Assert.Null(response2);
                Assert.Null(response3);
                
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region GetAllModulesForAnEmployee
        [Fact]
        public async void ModuleService_GetAllModulesForAnEmployee_When_ModulesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForAnEmployee(employee.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.NotEmpty(response);
                Assert.Equal(module.Title, response.First().Title);
                Assert.Equal(module.Body, response.First().Body);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_GetAllModulesForAnEmployee_When_NoModuleStatus()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForAnEmployee(employee.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Null(response3);
                Assert.Empty(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_GetAllModulesForAnEmployee_When_NoModulesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Modules = null
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(employee);
            db.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForAnEmployee(employee.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.Empty(response);
                Assert.NotNull(employer.Modules);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_GetAllModulesForAnEmployee_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForAnEmployee(employee.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Empty(response);
                Assert.Null(response2);
                Assert.Null(response3);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_GetAllModulesForAnEmployee_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };
            db.Add(employee);
            db.SaveChanges();

            try
            {
                // Act
                var response = await moduleService.GetAllModulesForAnEmployee(employee.Id);
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Empty(response);
                Assert.Null(response2);
                Assert.Null(response3);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region UpdateModuleStatusEmployee
        [Fact]
        public async void ModuleService_UpdateModuleStatus_When_ValidBehaviour()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string status = nameof(TrainingModuleStatus.Pending);

            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };
            
            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            string updatedStatus = nameof(TrainingModuleStatus.Completed);
            ModuleStatusInfo moduleStatusUpdated = new ModuleStatusInfo()
            {
                Id = moduleStatus.Id,
                Status = updatedStatus,
                Date = DateTime.Now,
            };

            try
            {
                // Act
                var response = await moduleService.UpdateModuleStatusEmployee(moduleStatusUpdated.Id, updatedStatus);
                var response2 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id && x.Module.Id == module.Id);

                // Assert
                Assert.True(response);
                Assert.Equal(updatedStatus, response2.Status);
             

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_UpdateModuleStatus_When_WrongStatus()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string status = nameof(TrainingModuleStatus.Pending);

            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            string updatedStatus = "RandomStatus";
            ModuleStatusInfo moduleStatusUpdated = new ModuleStatusInfo()
            {
                Id = moduleStatus.Id,
                Status = updatedStatus,
                Date = DateTime.Now,
            };

            try
            {
                // Act
                var response = await moduleService.UpdateModuleStatusEmployee(moduleStatusUpdated.Id, updatedStatus);

                // Assert
                Assert.False(response);
                



            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void ModuleService_UpdateModuleStatus_When_StatusDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string status = nameof(TrainingModuleStatus.Pending);

            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

            Module module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                Employer = employer,
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            ModuleStatus moduleStatus = new ModuleStatus()
            {
                Date = DateTime.Now,
                Employee = employee,
                Module = module,
                Status = nameof(TrainingModuleStatus.Completed)
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.SaveChanges();

            string updatedStatus = nameof(TrainingModuleStatus.Completed);
            ModuleStatusInfo moduleStatusUpdated = new ModuleStatusInfo()
            {
                Id = moduleStatus.Id,
                Status = updatedStatus,
                Date = DateTime.Now,
            };

            try
            {
                // Act
                var response = await moduleService.UpdateModuleStatusEmployee(moduleStatusUpdated.Id, updatedStatus);

                // Assert
                Assert.False(response);
                



            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

    }
}
