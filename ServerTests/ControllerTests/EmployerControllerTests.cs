using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Server.Data;
using Server.Services.Interfaces;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.BOModels;
using Newtonsoft.Json;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]

    public class EmployerControllerTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public EmployerControllerTests()
        {

        }
        #region Obsolete 
        [Fact]
        public async void EmployerController_CreateEmployeeObsolete_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userId = "Tst_000001";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"), 
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
             
                var response = await employerController.CreateEmployeeObsolete(employeeInfo,token.Token); 


                // Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }


        [Fact]
        public async void EmployerController_CreateEmployeeObsolete_When_SessionDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string forbidToken = "MEOW"; 

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.CreateEmployeeObsolete(employeeInfo,forbidToken);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_CreateEmployeeObsolete_When_EmployeeExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {   
                // Act
                var response = await employerController.CreateEmployeeObsolete(employeeInfo, token.Token);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_CreateEmployeeObsolete_When_FromUnauthorizedUser()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1 //change to make employee begave like an employer 
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.CreateEmployeeObsolete(employeeInfo, token.Token);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetEmployeeObsolete_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof"; 

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo)); 
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.GetEmployeeObsolete(randomId, token.Token);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetEmployeeObsolete_When_EmployeeDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.GetEmployeeObsolete(employeeInfo.Id, token.Token);

                // Assert
                Assert.NotNull(response.Value);
                Assert.Equal(employeeInfo.Id, response.Value.Id);
                Assert.Equal(employeeInfo.Email, response.Value.Email);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetEmployeeObsolete_When_ForbiddenPath()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            try
            {
                // Act
                var response = await employerController.GetEmployeeObsolete(employeeInfo.Id, token.Token);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result); 
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetEmployeeObsolete_When_AcessLevelIsNotEnough()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act

                var response = await employerController.GetEmployeeObsolete(employeeInfo.Id, token.Token);


                // Assert
                Assert.Null(response.Value);
                Assert.IsType<UnauthorizedResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployeeObsolete_When_SessionDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            UserInfo employeeInfoUpdated = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Jake Allen",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.UpdateEmployeeObsolete(employeeInfoUpdated, randomToken);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployeeObsolete_When_AccessLevelIsNotEnough()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.UpdateEmployeeObsolete(employeeInfo, token.Token);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<UnauthorizedResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployeeObsolete_When_EmployeeDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                SIN = ""
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0,
                SIN=""
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.UpdateEmployeeObsolete(employeeInfo, token.Token);

                // Assert
                Assert.NotNull(response.Value);
                Assert.Equal(employeeInfo.Id, response.Value.Id);
                Assert.Equal(employeeInfo.Email, response.Value.Email);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployeeObsolete_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.UpdateEmployeeObsolete(employeeInfo, token.Token);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_DeleteEmployeeObsolete_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.DeleteEmployeeObsolete(randomId, token.Token);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_DeleteEmployeeObsolete_When_EmployeeDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.DeleteEmployeeObsolete(employeeInfo.Id, token.Token);

                // Assert
                Assert.NotNull(response.Value);
                Assert.Equal(employeeInfo.Id, response.Value.Id);
                Assert.Equal(employeeInfo.Email, response.Value.Email);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_DeleteEmployeeObsolete_When_ForbiddenPath()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

          

            try
            {
                // Act
                var response = await employerController.DeleteEmployeeObsolete(employeeInfo.Id, token.Token);

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_DeleteEmployeeObsolete_When_UnauthorizedPath()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerController.DeleteEmployeeObsolete(employeeInfo.Id, token.Token);   

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<UnauthorizedResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetListOfEmployeesObsolete_When_EmployeesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();
            listOfEmployees.Add(employee);

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };


            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();



            try
            {
                // Act
                var response = await employerController.GetListOfEmployeesObsolete(token.User.Id);
                var response2 = db.Users.FirstOrDefault(x => x.Id == employer.Id);

                // Assert
                Assert.NotNull(response);
                Assert.Equal(employer.Email, response2.Email);
                Assert.Equal(employer.Id, response2.Id);
                Assert.NotNull(response2);
                Assert.NotEmpty(response2.Manages);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void EmployerController_GetListOfEmployeesObsolete_When_EmployeesDoNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };




            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();



            try
            {
                // Act
                var response = await employerController.GetListOfEmployeesObsolete(token.User.Id);
              

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
               


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void EmployerController_GetListOfEmployeesObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();
            listOfEmployees.Add(employee);

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };



            try
            {
                // Act
                var response = await employerController.GetListOfEmployeesObsolete(token.User.Id);
                var response2 = db.Users.FirstOrDefault(x => x.Id == employer.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
                Assert.Null(response2);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        #endregion

        #region GetEmployee

        [Fact]
        public async void EmployerController_GetEmployee_When_EmployeeDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000023";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new {sessionToken = token.Token, userId = employeeInfo.Id }));

            try
            {
                // Act
                var response = await employerController.GetEmployee(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<UserInfo>(un);
                Assert.Equal(employeeInfo.Id, responseValue.Id);
                Assert.Equal(employeeInfo.Email, responseValue.Email);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetEmployee_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = randomId }));

            try
            {
                // Act
                var response = await employerController.GetEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        [Fact]
        public async void EmployerController_GetEmployee_When_ForbiddenPath()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = employeeInfo.Id }));

            try
            {
                // Act
                var response = await employerController.GetEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_GetEmployee_When_AcessLevelIsNotEnough()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = employeeInfo.Id }));


            try
            {
                // Act

                var response = await employerController.GetEmployee(new EncryptedData(data));


                // Assert
                Assert.Null(response.Value);
                Assert.IsType<UnauthorizedResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        [Fact]
        public async void EmployerController_GetEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomData" }));


            try
            {
                // Act

                var response = await employerController.GetEmployee(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        #endregion

        #region GetListOfEmployees 

        [Fact]
        public async void EmployerController_GetListOfEmployees_When_EmployeesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();
            listOfEmployees.Add(employee);

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };


            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));



            try
            {
                // Act
                var response = await employerController.GetListOfEmployees(new EncryptedData(data));
                var response2 = db.Users.FirstOrDefault(x => x.Id == employer.Id);

                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<UserInfo>>(un);
                Assert.Equal(employee.Email, responseValue.First().Email);
                Assert.Equal(employee.Id, responseValue.First().Id);
                Assert.Equal(employer.Email, response2.Email);
                Assert.Equal(employer.Id, response2.Id);
                Assert.NotNull(response2);
                Assert.NotEmpty(response2.Manages);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void EmployerController_GetListOfEmployees_When_EmployeesDoNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };




            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));


            try
            {
                // Act
                var response = await employerController.GetListOfEmployees(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                Assert.Null(response.Result);



            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void EmployerController_GetListOfEmployees_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();
            listOfEmployees.Add(employee);

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.GetListOfEmployees(new EncryptedData(data));
                var response2 = db.Users.FirstOrDefault(x => x.Id == employer.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
                Assert.Null(response2);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void EmployerController_GetListOfEmployees_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,

            };

            ICollection<User> listOfEmployees = new List<User>();
            listOfEmployees.Add(employee);

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                Manages = listOfEmployees

            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomField" }));

            try
            {
                // Act
                var response = await employerController.GetListOfEmployees(new EncryptedData(data));
                

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
               


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }
        #endregion

        #region CreateEmployee
        [Fact]
        public async void EmployerController_CreateEmployee_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000001";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = token.Token }));

            try
            {
                // Act

                var response = await employerController.CreateEmployee(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }


        [Fact]
        public async void EmployerController_CreateEmployee_When_SessionDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string forbidToken = "MEOW";

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = forbidToken }));

            try
            {
                // Act
                var response = await employerController.CreateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_CreateEmployee_When_EmployeeExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.CreateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_CreateEmployee_When_FromUnauthorizedUser()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1 //change to make employee begave like an employer 
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.CreateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        [Fact]
        public async void EmployerController_CreateEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1 //change to make employee begave like an employer 
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomData" }));

            try
            {
                // Act
                var response = await employerController.CreateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_CreateEmployee_When_InvalidUserInfoSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1 //change to make employee begave like an employer 
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = "randomData", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.CreateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }



        #endregion

        #region UpdateEmployee


        [Fact]
        public async void EmployerController_UpdateEmployee_When_EmployeeDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1,
                SIN = ""
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0,
                SIN = ""
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.UpdateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<UserInfo>(un);
                Assert.Equal(employeeInfo.Id, responseValue.Id);
                Assert.Equal(employeeInfo.Email, responseValue.Email);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }


        [Fact]
        public async void EmployerController_UpdateEmployee_When_SessionDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            UserInfo employeeInfoUpdated = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Jake Allen",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfoUpdated, sessionToken = randomToken }));

            try
            {
                // Act
                var response = await employerController.UpdateEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployee_When_AccessLevelIsNotEnough()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.UpdateEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<UnauthorizedResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployee_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = employeeInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.UpdateEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1 //change to make employee begave like an employer 
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomData" }));

            try
            {
                // Act
                var response = await employerController.UpdateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_UpdateEmployee_When_InvalidUserInfoSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1 //change to make employee begave like an employer 
            };

            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { employee = "randomData", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await employerController.UpdateEmployee(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }



        #endregion

        #region DeleteEmployee

        [Fact]
        public async void EmployerController_DeleteEmployee_When_EmployeeDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000023";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = employeeInfo.Id }));

            try
            {
                // Act
                var response = await employerController.DeleteEmployee(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<UserInfo>(un);
                Assert.Equal(employeeInfo.Id, responseValue.Id);
                Assert.Equal(employeeInfo.Email, responseValue.Email);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_DeleteEmployee_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = randomId }));

            try
            {
                // Act
                var response = await employerController.DeleteEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        [Fact]
        public async void EmployerController_DeleteEmployee_When_ForbiddenPath()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 0
            };

            string randomToken = "woof";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = employeeInfo.Id }));

            try
            {
                // Act
                var response = await employerController.DeleteEmployee(new EncryptedData(data));

                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void EmployerController_DeleteEmployee_When_AcessLevelIsNotEnough()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, userId = employeeInfo.Id }));


            try
            {
                // Act

                var response = await employerController.DeleteEmployee(new EncryptedData(data));


                // Assert
                Assert.Null(response.Value);
                Assert.IsType<UnauthorizedResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        [Fact]
        public async void EmployerController_DeleteEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            EmployerController employerController = new EmployerController(employerService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            string randomId = "Tst_6969";
            User employer = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            UserInfo employeeInfo = new UserInfo()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user"),
                AccessLevel = 1
            };

            string randomToken = "woof";

            db.Users.Add(employer);
            db.Users.Add(new User(employeeInfo));
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomData" }));


            try
            {
                // Act

                var response = await employerController.DeleteEmployee(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion







    }
}
