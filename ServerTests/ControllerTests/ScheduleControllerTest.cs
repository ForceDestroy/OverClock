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

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class ScheduleControllerTest : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public ScheduleControllerTest()
        {

        }
        #region Obsolete
        [Fact]
        public async void ScheduleController_ModifyScheduleObsolete_When_ScheduleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            try
            {
                // Act
                var response = await scheduleController.ModifyScheduleObsolete(scheduleInfoList, employee.Id, token.Token);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotEqual(startTime, response2.StartTime);
                Assert.NotEqual(endTime, response2.EndTime);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifyScheduleObsolete_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };


            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };


            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);
            try
            {
                // Act
                var response = await scheduleController.ModifyScheduleObsolete(scheduleInfoList, employee.Id, token.Token);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifyScheduleObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
            };


            db.Users.Add(employee);
            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);
            try
            {
                // Act
                var response = await scheduleController.ModifyScheduleObsolete(scheduleInfoList, employee.Id, token.Token);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifyScheduleObsolete_When_SessionIsUnauthorized()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0
            };


            db.Users.Add(employee);
            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,    
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);
            try
            {
                // Act
                var response = await scheduleController.ModifyScheduleObsolete(scheduleInfoList, employee.Id, token.Token);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_GetAllSchedulesForEmployeeObsolete_When_ScheduleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

           
            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

           

            try
            {
                // Act
                var response = await scheduleController.GetAllSchedulesForEmployeeObsolete(token.Token);
                var response2 = db.Users.FirstOrDefault(x => x.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.True(DateTime.Compare(date, response.Value.First().Date) == 0);
                Assert.True(DateTime.Compare(startTime, response.Value.First().StartTime) == 0);
                Assert.NotNull(response2);
                Assert.NotEmpty(response2.Schedules);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }


        [Fact]
        public async void ScheduleController_GetAllSchedulesForEmployeeObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

  

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
            };


            db.Users.Add(employee);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleController.GetAllSchedulesForEmployeeObsolete(token.Token);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployeeObsolete_When_SchedulesDoExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>(),
                Manager = employer
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployeeObsolete(employee.Id, token.Token, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.True(DateTime.Compare(date, response.Value.First().Date) == 0);
                Assert.True(DateTime.Compare(date, response2.Date) == 0);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployeeObsolete_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>(),
                Manager = employer
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployeeObsolete(employee.Id, token.Token, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployeeObsolete_When_NotAuthorized()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 0,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployeeObsolete(employee.Id, token.Token, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response.Result);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region ModifySchedule
        [Fact]
        public async void ScheduleController_ModifySchedule_When_ScheduleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { scheduleSubmissions = scheduleInfoList, sessionToken = token.Token, userId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.ModifySchedule(new EncryptedData(data));
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.NotEqual(startTime, response2.StartTime);
                Assert.NotEqual(endTime, response2.EndTime);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifySchedule_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };


            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };


            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { scheduleSubmissions = scheduleInfoList, sessionToken = token.Token, userId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.ModifySchedule(new EncryptedData(data));
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifySchedule_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0
            };


            db.Users.Add(employee);
            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { scheduleSubmissions = scheduleInfoList, sessionToken = token.Token, userId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.ModifySchedule(new EncryptedData(data));
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifySchedule_When_SessionIsUnauthorized()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0
            };


            db.Users.Add(employee);
            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { scheduleSubmissions = scheduleInfoList, sessionToken = token.Token, userId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.ModifySchedule(new EncryptedData(data));
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifySchedule_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomData" }));

            try
            {
                // Act
                var response = await scheduleController.ModifySchedule(new EncryptedData(data));
                
                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_ModifySchedule_When_InvalidScheduleListSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            // Making the updatedSchedule after so it can inherit the proper id
            Schedule updatedSchedule = new Schedule()
            {
                Id = schedule.Id,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 17, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 21, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            ScheduleInfo scheduleInfo1 = new ScheduleInfo(updatedSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { scheduleSubmissions = "randomData", sessionToken = token.Token, userId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.ModifySchedule(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }
        #endregion

        #region GetAllSchedulesForEmployee
        [Fact]
        public async void ScheduleController_GetAllSchedulesForEmployee_When_ScheduleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };


            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await scheduleController.GetAllSchedulesForEmployee(new EncryptedData(data));
                var response2 = db.Users.FirstOrDefault(x => x.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<ScheduleInfo>>(un);
                Assert.True(DateTime.Compare(date, responseValue.First().Date) == 0);
                Assert.True(DateTime.Compare(startTime, responseValue.First().StartTime) == 0);
                Assert.NotNull(response2);
                Assert.NotEmpty(response2.Schedules);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_GetAllSchedulesForEmployee_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };



            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0
            };


            db.Users.Add(employee);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await scheduleController.GetAllSchedulesForEmployee(new EncryptedData(data));
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void ScheduleController_GetAllSchedulesForEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                Schedules = new List<Schedule>()
            };


            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule);


            db.Users.Add(employee);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomData" }));

            try
            {
                // Act
                var response = await scheduleController.GetAllSchedulesForEmployee(new EncryptedData(data));
                var response2 = db.Users.FirstOrDefault(x => x.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }
        #endregion

        #region GetSchedulesForAnEmployee

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployee_When_SchedulesDoExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>(),
                Manager = employer
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();


            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { date = date, sessionToken = token.Token, employeeId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployee(new EncryptedData(data));
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<ScheduleInfo>>(un);
                Assert.True(DateTime.Compare(date, responseValue.First().Date) == 0);
                Assert.True(DateTime.Compare(date, response2.Date) == 0);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployee_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>(),
                Manager = employer
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SaveChanges();
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { date = date, sessionToken = token.Token, employeeId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployee(new EncryptedData(data));
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployee_When_NotAuthorized()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 0,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>()
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { date = date, sessionToken = token.Token, employeeId = employee.Id }));

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployee(new EncryptedData(data));
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response.Result);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleController_GetSchedulesForAnEmployee_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            ISessionService sessionService = new SessionService(db);
            IEmployerService employerService = new EmployerService(db);
            ScheduleController scheduleController = new ScheduleController(scheduleService, sessionService, employerService);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Schedules = new List<Schedule>(),
                Manager = employer
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Users.Add(employer);
            db.SessionTokens.Add(token);
            db.Schedules.Add(schedule);
            db.SaveChanges();


            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomValue" }));

            try
            {
                // Act
                var response = await scheduleController.GetSchedulesForAnEmployee(new EncryptedData(data));
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion
    }
}
