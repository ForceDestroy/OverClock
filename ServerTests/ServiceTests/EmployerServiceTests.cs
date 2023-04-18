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

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class EmployerServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public EmployerServiceTests()
        {

        }
        
        [Fact]
        public async void EmployerService_CreateUser_When_EmployeeDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
            db.Database.ExecuteSqlRaw("delete from Modules");
            db.Database.ExecuteSqlRaw("delete from ModuleStatus");
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange
            string password = "password";
            UserInfo user = new UserInfo()
            {
                Id = "Tst",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = password
            };

            User employer = new User()
            {
                Id = "Tst_000005",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle"
            };

            Module module = new()
            {
                Employer = employer,
                Title = "Some title",
                Body = "Some important text",
                Date = DateTime.Now,
                Statuses = new List<ModuleStatus>()
            };

            db.Users.Add(employer);
            db.Modules.Add(module);
            db.SaveChanges();

            string expectedUserId = "Tst_000006";
            try
            {
                // Act
                var response = await employerService.CreateEmployee(user, employer.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(u => u.Id == expectedUserId);
                var response3 = await db.Modules.Include(x => x.Statuses).FirstOrDefaultAsync(x => x.Employer.Id == employer.Id);
                var response4 = await db.ModuleStatus.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Employee.Id == expectedUserId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(user.Email, response2.Email);
                Assert.Equal(employer, response2.Manager);
                Assert.Equal(expectedUserId, response2.Id);
                Assert.NotNull(response3);
                Assert.NotNull(response4);
                Assert.Equal(module.Body, response3.Body);
                Assert.NotEmpty(response3.Statuses);
                Assert.Equal(nameof(TrainingModuleStatus.Unseen), response4.Status);
                Assert.Equal(module.Id, response4.Module.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Modules");
                db.Database.ExecuteSqlRaw("delete from ModuleStatus");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void EmployerService_CreateEmployee_When_EmailExists()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            string password = "password";
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = password
            };

            UserInfo userInfo = new UserInfo()
            {
                Id = "Tst_000001",
                Name = "John Smith2",
                Email = "test@gmail.com",
                Password = password
            };

            User employer = new User()
            {
                Id = "Tst_000005",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle"
            };

            db.Users.Add(employer);
            db.Users.Add(user);
            db.SaveChanges();

            try
            {
                //Act
                var response = await employerService.CreateEmployee(userInfo, employer.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                //Assert
                Assert.False(response);
                Assert.Equal(user.Id, response2.Id);
                Assert.Equal(userInfo.Email, response2.Email);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }


        [Fact]
        public async void EmployerService_GetEmployee_When_EmployeeExists()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            string password = "password";
            UserInfo userInfo = new UserInfo()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = password
            };

            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = password
            };

            db.Users.Add(user);
            db.SaveChanges();

            try
            {
                //Act
                var response = await employerService.GetEmployee(user.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Equal(user.Id, response.Id);
                Assert.Equal(user.Email, response.Email);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }

        [Fact]
        public async void EmployerService_GetEmployee_When_EmployeeDoesNotExist()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange

            string password = "password"; 
            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = password 
            };

            try
            {
                //Act
                var response = await employerService.GetEmployee(user.Id);

                //Assert
                Assert.Null(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }


        }

        [Fact]
        async public void Employer_UpdateEmployee_When_EmployeeExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            string password = "password";
            string updatedPassword = "password12"; 
            UserInfo user = new UserInfo()
            {
                Id = "Tst_000002",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = password,
                SIN=""
            };

            db.Users.Add(new User(user));
            db.SaveChanges();

            string updatedUserId = "Tst_000002";
            UserInfo updatedUser = new UserInfo()
            {
                Id = updatedUserId,
                Name = "Aiman Hanna",
                Email = "test@gmail.com",
                Password = updatedPassword,
                SIN = ""
            };

            try
            {
                // Act
                var response = await employerService.UpdateEmployee(updatedUser);

                // Assert
                Assert.NotNull(response);
                Assert.Equal(updatedUser.Email, response.Email);
                Assert.Equal(updatedUser.Id, response.Id);
                Assert.Equal(updatedUser.Name, response.Name);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }

        [Fact]
        async public void Employer_UpdateEmployee_When_EmployeeDoesNotExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange

            string beforeUpdatePswd = "hellokitty123";
            UserInfo user = new UserInfo()
            {
                Id = "Tst_000002",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = beforeUpdatePswd
            };

           

            string updatedUserId = "Tst_000002";
            string afterUpdatePswd = "jamescharles00";
            UserInfo updatedUser = new UserInfo()
            {
                Id = updatedUserId,
                Name = "Aiman Hanna",
                Email = "test@gmail.com",
                Password = afterUpdatePswd
            };

            try
            {
                // Act
                var response = await employerService.UpdateEmployee(updatedUser);
   

                // Assert
                Assert.Null(response);
                

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }

        [Fact]
        async public void Employer_UpdateEmployee_When_EmployeeNewEmailAlreadyExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange

            string beforeUpdatePswd = "hellokitty123";
            UserInfo user = new UserInfo()
            {
                Id = "Tst_000002",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = beforeUpdatePswd
            };



            string updatedUserId = "Tst_000002";
            string afterUpdatePswd = "jamescharles00";
            UserInfo updatedUser = new UserInfo()
            {
                Id = updatedUserId,
                Name = "Aiman Hanna",
                Email = "test1@gmail.com",
                Password = afterUpdatePswd
            };

            UserInfo randomUser = new UserInfo()
            {
                Id = "Tst_00087",
                Name = "Aiman Hanna",
                Email = "test1@gmail.com",
                Password = afterUpdatePswd
            };

            db.Users.Add(new User(user));
            db.Users.Add(new User(randomUser));
            db.SaveChanges();
            try
            {
                // Act
                var response = await employerService.UpdateEmployee(updatedUser);


                // Assert
                Assert.Null(response);
             


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }



        [Fact]
        public async void EmployerService_DeleteEmployee_When_EmployeeExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db); 
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
            db.Database.ExecuteSqlRaw("delete from Payslips");
            db.Database.ExecuteSqlRaw("delete from Requests");
            db.Database.ExecuteSqlRaw("delete from Schedules");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
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

            employee.Schedules.Add(schedule);

            Request request = new Request()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Status = RequestStatus.Pending.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            PaySlip payslip = new PaySlip()
            {
                User = employee,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
            };

            WorkHours workHours = new WorkHours()
            {
                User = employee,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 10, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Payslips.Add(payslip);
            db.Requests.Add(request);
            db.Schedules.Add(schedule);
            db.WorkHours.Add(workHours);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.DeleteEmployee(employee.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(x => x.Id == employee.Id);

                // Assert
                Assert.NotNull(response);
                Assert.Equal(userEmail, response.Email);
                Assert.Equal(userId, response.Id);
                Assert.Null(response2);
                Assert.Empty(db.Payslips);
                Assert.Empty(db.Requests);
                Assert.Empty(db.Schedules);
                Assert.Empty(db.WorkHours);

            }
            finally
            {
                // Dispose

                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Payslips");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");

            }


        }

        [Fact]
        public async void EmployerService_DeleteEmployee_When_EmployeeDoesNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange      
            string testId = "Tst_000003";
            string Name = "Shawn Gorman";
            string email = "test@gmail.com";
            string password = "password";
            UserInfo user = new UserInfo()
            {
                Id = testId,
                Name = Name,
                Email = email,
                Password = password
            };

          
            try
            {
                // Act
                var response = await employerService.DeleteEmployee(user.Email);
               

                // Assert
                Assert.Null(response);
                
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }


        }

        [Fact]
        public async void EmployerService_ValidateAuthorization_When_ValidAuthorization()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.ValidateAuthorization(employer.Id, employee.Id);


                // Assert
                Assert.True(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void EmployerService_ValidateAuthorization_When_InvalidAuthorization()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.ValidateAuthorization(employer.Id, employee.Id);


                // Assert
                Assert.False(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void EmployerService_ValidateAuthorization_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            db.Users.Add(employee);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.ValidateAuthorization(employer.Id, employee.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(x => x.Id == employer.Id);

                // Assert
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
        public async void EmployerService_ValidateAuthorization_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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

            User employer = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1,
                Schedules = new List<Schedule>()
            };

            db.Users.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.ValidateAuthorization(employer.Id, employee.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(x => x.Id == employee.Id);

                // Assert
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
        public async void EmployerService_GetListOfEmployees_When_EmployerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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



            db.Users.Add(employee); 
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.GetListOfEmployees(employer.Id); 
                // Assert
                Assert.Null(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void EmployerService_GetListOfEmployees_When_EmployeesDoNotExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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

            try
            {
                // Act
                var response = await employerService.GetListOfEmployees(employer.Id);
                // Assert
                Assert.Null(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void EmployerService_GetListOfEmployees_When_EmployeesDoExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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



            db.Users.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await employerService.GetListOfEmployees(employer.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(x => x.Id == employer.Id);

                // Assert
                Assert.NotNull(response);
                Assert.NotEmpty(response);
                Assert.Equal(employer.Email, response2.Email);
                Assert.Equal(employer.Id, response2.Id);
                Assert.NotEmpty(response2.Manages);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Schedules");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void EmployerService_GetAllSchedulesForEmployee_When_ListIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
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
                Manages = null

            };

         



            try
            {
                // Act
                var response = await employerService.GetListOfEmployees(employer.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(x => x.Id == employer.Id);

                // Assert
                Assert.Null(response);
           


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void EmployerService_CreateEmployee_When_WhenExistingIDsPresent()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = "password"
            };

            User user1 = new User()
            {
                Id = "Tst_000200",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = "password"
            };

            User user2 = new User()
            {
                Id = "Tst_200000",
                Name = "John Smith",
                Email = "test2@gmail.com",
                Password = "password"
            };

            User user3 = new User()
            {
                Id = "Tst_358691",
                Name = "John Smith",
                Email = "test3@gmail.com",
                Password = "password"
            };

            User user4 = new User()
            {
                Id = "OTT_400000",
                Name = "John Smith",
                Email = "test4@gmail.com",
                Password = "password"
            };

            db.Users.Add(user);
            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Users.Add(user3);
            db.Users.Add(user4);
            db.SaveChanges();

            UserInfo userInfo = new UserInfo()
            {
                Id = "Tst_000001",
                Name = "TESTNAME",
                Email = "test5@gmail.com",
                Password = "password"
            };

            try
            {
                //Act
                var response = await employerService.CreateEmployee(userInfo, user.Id);
                var response2 = await db.Users.FirstOrDefaultAsync(u => u.Name == userInfo.Name);

                //Assert
                Assert.True(response);
                Assert.Equal("Tst_358692", response2.Id);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }
    }
}

