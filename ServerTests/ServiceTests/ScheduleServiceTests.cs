using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class ScheduleServiceTests
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public ScheduleServiceTests()
        {

        }

        [Fact]
        async public void ScheduleService_ModifySchedule_When_ScheduleExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            ScheduleInfo scheduleInfo = new ScheduleInfo(updatedSchedule);
            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo);

            try
            {
                // Act
                var response = await scheduleService.ModifySchedule(scheduleInfoList, employee.Id);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.NotEqual(startTime, response2.StartTime);
                Assert.NotEqual(endTime, response2.EndTime);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        async public void ScheduleService_ModifySchedule_When_MoreSubmittedSchedules()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            Schedule newSchedule1 = new Schedule()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };
            Schedule newSchedule2 = new Schedule()
            {
                Date = new DateTime(2020, 02, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 02, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 02, 01, 10, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            
            ScheduleInfo scheduleInfo1 = new ScheduleInfo(newSchedule1);
            ScheduleInfo scheduleInfo2 = new ScheduleInfo(newSchedule2);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);
            scheduleInfoList.Add(scheduleInfo2);

            try
            {
                // Act
                var response = await scheduleService.ModifySchedule(scheduleInfoList, employee.Id);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.NotEqual(startTime, response2.StartTime);
                Assert.NotEqual(endTime, response2.EndTime);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        [Fact]
        async public void ScheduleService_ModifySchedule_When_MoreCurrentSchedules()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule1 = new Schedule()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                AllowedBreakTime = 0,
                User = employee
            };
            Schedule schedule2 = new Schedule()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            employee.Schedules.Add(schedule1);
            employee.Schedules.Add(schedule2);

            
            Schedule newSchedule = new Schedule()
            {
                Date = new DateTime(2020, 01, 02, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 02, 12, 00, 00),
                EndTime = new DateTime(2020, 01, 02, 13, 00, 00),
                AllowedBreakTime = 0,
                User = employee
            };

            db.Users.Add(employee);
            db.Schedules.Add(schedule1);
            db.Schedules.Add(schedule2);
            db.SaveChanges();


            ScheduleInfo scheduleInfo1 = new ScheduleInfo(newSchedule);

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo1);

            try
            {
                // Act
                var response = await scheduleService.ModifySchedule(scheduleInfoList, employee.Id);
                var response2 = db.Schedules.First();

                // Assert
                Assert.NotNull(response);
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.NotEqual(startTime, response2.StartTime);
                Assert.NotEqual(endTime, response2.EndTime);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        async public void ScheduleService_UpdateSchedule_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            ScheduleInfo scheduleInfo = new ScheduleInfo(updatedSchedule);
            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();
            scheduleInfoList.Add(scheduleInfo);
            try
            {
                // Act
                var response = await scheduleService.ModifySchedule(scheduleInfoList, employee.Id);
                var response2 = db.Schedules.FirstOrDefault(schedule => schedule.User.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.False(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleService_GetAllSchedulesForEmployee_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            db.Schedules.Add(schedule); 
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleService.GetAllSchedulesForEmployee(employee.Id);
                // Assert
                Assert.Empty(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleService_GetAllSchedulesForEmployee_When_SchedulesDoNotExist()

        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleService.GetAllSchedulesForEmployee(employee.Id);
                // Assert
                Assert.Empty(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleService_GetAllSchedulesForEmployee_When_SchedulesDoExist()

        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            DateTime date = new DateTime(2020, 01, 01, 00, 00, 00);
            DateTime startTime = new DateTime(2020, 01, 01, 09, 00, 00);
            DateTime endTime = new DateTime(2020, 01, 01, 10, 00, 00);

            Schedule schedule = new Schedule()
            {
                User = employee,
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
                var response = await scheduleService.GetAllSchedulesForEmployee(employee.Id);
                var response2 = db.Users.FirstOrDefault(x => x.Id == employee.Id);
                // Assert
                Assert.NotNull(response);
                Assert.NotEmpty(response);
                Assert.True(DateTime.Compare(date, response.First().Date) == 0);
                Assert.True(DateTime.Compare(startTime, response.First().StartTime) == 0);
                Assert.NotEmpty(response2.Schedules); 

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleService_GetSchedulesForAnEmployee_When_SchedulesDoExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                var response = await scheduleService.GetSchedulesForAnEmployee(employee.Id, employer.Id, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.NotNull(response);
                Assert.True(DateTime.Compare(date, response2.Date) == 0);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleService_GetSchedulesForAnEmployee_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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

            db.Users.Add(employer);
            db.Schedules.Add(schedule);
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleService.GetSchedulesForAnEmployee(employee.Id, employer.Id, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.Empty(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }
        
        [Fact]
        public async void ScheduleService_GetSchedulesForAnEmployee_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
            db.Schedules.Add(schedule);
            db.SaveChanges();

            try
            {
                // Act
                var response = await scheduleService.GetSchedulesForAnEmployee(employee.Id, employer.Id, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.Empty(response);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void ScheduleService_GetSchedulesForAnEmployee_When_EmployerDoesntManageEmployee()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IScheduleService scheduleService = new ScheduleService(db);
            db.Database.ExecuteSqlRaw("delete from Schedules");
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
                var response = await scheduleService.GetSchedulesForAnEmployee(employee.Id, employer.Id, date);
                var response2 = db.Schedules.Include(x => x.User).First();
                // Assert
                Assert.Empty(response);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

    }
}
