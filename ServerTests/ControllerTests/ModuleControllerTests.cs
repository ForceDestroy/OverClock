using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using Newtonsoft.Json;
using Server.Enums;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class ModuleControllerTests : ControllerBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public ModuleControllerTests()
        {

        }

        #region CreateModule
        [Fact]
        public async void ModuleController_CreateModule_When_ValidBehaviour()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            ModuleInfo module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                EmployerId = employer.Id,
                Date = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleInfo = module, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.CreateModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<OkResult>(response);
                Assert.Equal(module.Title, response2.Title);
                Assert.Equal(module.Body, response2.Body);
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
        public async void ModuleController_CreateModule_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SaveChanges();

            ModuleInfo module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                EmployerId = employer.Id,
                Date = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleInfo = module, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.CreateModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.IsType<NotFoundResult>(response);
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
        public async void ModuleController_CreateModule_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            ModuleInfo module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                EmployerId = employer.Id,
                Date = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { smtg = "smtg", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.CreateModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.IsType<BadRequestObjectResult>(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void ModuleController_CreateModule_When_InvalidModuleInfoSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            ModuleInfo module = new()
            {
                Body = "Some valuable text",
                Title = "Some significant title",
                EmployerId = employer.Id,
                Date = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleInfo = "string", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.CreateModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.IsType<BadRequestObjectResult>(response);
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
        public async void ModuleController_DeleteModule_When_ModuleExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleId = module.Id, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.DeleteModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.IsType<OkResult>(response);

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
        public async void ModuleController_DeleteModule_When_ModuleDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(employee);
            db.Add(employer);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleId = module.Id, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.DeleteModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Null(response2);
                Assert.Null(response3);
                Assert.IsType<BadRequestResult>(response);
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
        public async void ModuleController_DeleteModule_When_InvalidSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleId = module.Id, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.DeleteModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<NotFoundResult>(response);
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
        public async void ModuleController_DeleteModule_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomfield = "randomvalue" }));

            try
            {
                // Act
                var response = await moduleController.DeleteModule(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<BadRequestObjectResult>(response);
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
        public async void ModuleController_GetAllModuledForEmployer_When_ModulesExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.GetAllModulesForEmployer(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<ModuleSummary>>(un);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.Equal(module.Title, responseValue.First().Title);
                Assert.Equal(module.Body, responseValue.First().Body);
                Assert.Equal(module.Employer.Id, responseValue.First().EmployerId);
                Assert.NotEmpty(responseValue.First().Statuses);
                Assert.Equal(nameof(TrainingModuleStatus.Completed), responseValue.First().Statuses.First().Status);
                Assert.NotEmpty(response2.Statuses);
                Assert.Equal(nameof(TrainingModuleStatus.Completed), response3.Status);
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
        public async void ModuleController_GetAllModuledForEmployer_When_InvalidSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.GetAllModulesForEmployer(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<NotFoundResult>(response.Result);
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
        public async void ModuleController_GetAllModuledForEmployer_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomValue" }));

            try
            {
                // Act
                var response = await moduleController.GetAllModulesForEmployer(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<BadRequestObjectResult>(response.Result);
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
        public async void ModuleController_GetAllModuledForAnEmployee_When_ModulesExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.GetAllModulesForAnEmployee(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<ModuleInfo>>(un);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.Equal(module.Title, responseValue.First().Title);
                Assert.Equal(module.Body, responseValue.First().Body);
                Assert.Equal(module.Employer.Id, responseValue.First().EmployerId);
                Assert.NotEmpty(response2.Statuses);
                Assert.Equal(nameof(TrainingModuleStatus.Completed), response3.Status);
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
        public async void ModuleController_GetAllModuledForAnEmployee_When_InvalidSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await moduleController.GetAllModulesForAnEmployee(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<NotFoundResult>(response.Result);
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
        public async void ModuleController_GetAllModuledForAnEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomValue" }));

            try
            {
                // Act
                var response = await moduleController.GetAllModulesForEmployer(new EncryptedData(data));
                var response2 = db.Modules.Include(x => x.Employer).Include(x => x.Statuses).FirstOrDefault(x => x.Employer.Id == employer.Id);
                var response3 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.IsType<BadRequestObjectResult>(response.Result);

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
        public async void ModuleController_UpdateModuleStatus_When_ValidBehaviour()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token); 
            db.SaveChanges();

            string updatedStatus = nameof(TrainingModuleStatus.Completed);
            ModuleStatusInfo moduleStatusUpdated = new ModuleStatusInfo()
            {
                Id = moduleStatus.Id,
                Status = updatedStatus,
                Date = DateTime.Now,
            };

            string randomToken = "woof";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleStatusId = moduleStatusUpdated.Id, sessionToken = token.Token, status = updatedStatus}));

            try
            {
                // Act
                var response = await moduleController.UpdateModuleStatusEmployee(new EncryptedData(data));
                var response2 = db.ModuleStatus.FirstOrDefault(x => x.Employee.Id == employee.Id && x.Module.Id == module.Id);


                // Assert
                Assert.NotNull(response);
                Assert.Equal(moduleStatusUpdated.Id, response2.Id);
                Assert.Equal(moduleStatusUpdated.Status, response2.Status);




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
        public async void ModuleController_UpdateModuleStatus_When_SessionDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
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

            string randomToken = "woof";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { moduleStatusId = moduleStatusUpdated.Id, sessionToken = randomToken, status = updatedStatus }));

            try
            {
                // Act
                var response = await moduleController.UpdateModuleStatusEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);




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
        public async void ModuleController_UpdateModuleStatus_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IModuleService moduleService = new ModuleService(db);
            ISessionService sessionService = new SessionService(db);
            ModuleController moduleController = new ModuleController(moduleService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Add(module);
            db.Add(employee);
            db.Add(employer);
            db.Add(moduleStatus);
            db.Add(token);
            db.SaveChanges();

            string updatedStatus = nameof(TrainingModuleStatus.Completed);
            ModuleStatusInfo moduleStatusUpdated = new ModuleStatusInfo()
            {
                Id = moduleStatus.Id,
                Status = updatedStatus,
                Date = DateTime.Now,
            };

            string randomToken = "woof";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomInfo = "randomInfo"}));

            try
            {
                // Act
                var response = await moduleController.UpdateModuleStatusEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);

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
