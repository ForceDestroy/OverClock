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

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class SysAdminControllerTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        


        public SysAdminControllerTests()
        {
            
        }

        [Fact]
        public void SysAdminController_CreateUser_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            string password = "password";
            UserInfo user = new UserInfo()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = password
            };

            try
            {
                // Act
                var response1 = db.Users.FirstOrDefault(u => u.Id == user.Id);
                controller.CreateUser(user);
                var response2 = db.Users.FirstOrDefault(u => u.Id == user.Id);

                // Assert
                Assert.Null(response1);
                Assert.NotNull(response2);
                Assert.Equal(user.Email, response2.Email);
                Assert.Equal(user.Id, response2.Id);
                Assert.Equal(password, EncryptionHelper.DecryptString(response2.Password));

            }
            finally
            {

                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }

        [Fact]
        public void SysAdminController_CreateUser_When_UserExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
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
                Id = userInfo.Id,
                Name = userInfo.Name,
                Email = userInfo.Email,
                Password = EncryptionHelper.EncryptString(userInfo.Password)
            };

            db.Add(user);
            db.SaveChanges();

            try
            {
                // Act
                var response1 = controller.CreateUser(userInfo);
                var response2 = db.Users.FirstOrDefault(u => u.Id == userInfo.Id);

                // Assert
                Assert.IsType<BadRequestResult>(response1);
                Assert.NotNull(response2);
                Assert.Equal(user.Email, response2.Email);
                Assert.Equal(user.Id, response2.Id);
                Assert.Equal(password, EncryptionHelper.DecryptString(response2.Password));


            }
            finally
            {

                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }

        [Fact]
        public void SysAdminController_GetUser_When_UserExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(user);
            db.SaveChanges();

            try
            {

                // Act
                var response = controller.GetUser(user.Id).Value;

                // Assert
                Assert.NotNull(response);
                Assert.Equal(user.Email, response.Email);
                Assert.Equal(user.Id, response.Id);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }

        }

        [Fact]
        public void SysAdminController_GetUser_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };


            try
            {

                // Act
                var response = controller.GetUser(user.Id);

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
        public void SysAdminController_UpdateUser_When_UserExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            UserInfo user = new UserInfo()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(new User(user));
            db.SaveChanges();

            string updatedPW = "A car is a vehicle";
            UserInfo updatedUser = new UserInfo()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test@gmail.com",
                Password = updatedPW
            };

            try
            {
                // Act
                var response = controller.UpdateUser(updatedUser).Value;

                // Assert
                Assert.NotNull(response);
                Assert.Equal(updatedUser.Email, response.Email);
                Assert.Equal(updatedUser.Id, response.Id);
                Assert.Equal(updatedUser.Name, response.Name);
                Assert.Equal(updatedPW, EncryptionHelper.DecryptString(response.Password));

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }


        }

        [Fact]
        public void SysAdminController_UpdateUser_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            UserInfo user = new UserInfo()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            

            string updatedPW = "A car is a vehicle";
            UserInfo updatedUser = new UserInfo()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test@gmail.com",
                Password = updatedPW
            };

            try
            {
                // Act
                var response = controller.UpdateUser(updatedUser);

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
        public void SysAdminController_DeleteUser_When_UserExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(user);
            db.SaveChanges();

            try
            {
                // Act
                var response = controller.DeleteUser(user.Id).Value;
                var response2 = db.Users.FirstOrDefault(x => x.Id == user.Id);

                // Assert
                Assert.NotNull(response);
                Assert.Equal(user.Email, response.Email);
                Assert.Equal(user.Id, response.Id);
                Assert.Equal(user.Name, response.Name);
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
        public void SysAdminController_DeleteUser_When_UserDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController controller = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            try
            {
                // Act
                var response = controller.DeleteUser(user.Id);

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
        public void SysAdminController_SetManagerEmployeeRelationship_When_EmployeeAndManagerExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            db.Users.Add(manager);
            db.Users.Add(employee);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.SetManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(manager.Id, response2.Manager.Id);
                Assert.Contains(employee, response3.Manages);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

        [Fact]
        public void SysAdminController_SetManagerEmployeeRelationship_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            db.Users.Add(manager);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.SetManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Null(response2);
                Assert.NotNull(response3);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

        [Fact]
        public void SysAdminController_SetManagerEmployeeRelationship_When_ManagerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            db.Users.Add(employee);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.SetManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.NotNull(response2);
                Assert.Null(response3);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

        [Fact]
        public void SysAdminController_SetManagerEmployeeRelationship_When_RelationshipAlreadyExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      
            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = manager
            };

            db.Users.Add(manager);
            db.Users.Add(employee);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.SetManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

        [Fact]
        public void SysAdminController_RemoveManagerEmployeeRelationship_When_EmployeeAndManagerExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      


            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = manager
            };

            db.Users.Add(manager);
            db.Users.Add(employee);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.RemoveManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.Null(response2.Manager);
                Assert.DoesNotContain(employee, response3.Manages);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

        [Fact]
        public void SysAdminController_RemoveManagerEmployeeRelationship_When_EmployeeDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      

            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = manager
            };

            db.Users.Add(manager);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.RemoveManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Null(response2);
                Assert.NotNull(response3);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

        [Fact]
        public void SysAdminController_RemoveManagerEmployeeRelationship_When_ManagerDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISysAdminService sysAdminService = new SysAdminService(db);
            SysAdminController sysAdminController = new SysAdminController(sysAdminService);
            db.Database.ExecuteSqlRaw("delete from Users");


            // Arrange      

            User manager = new User()
            {
                Id = "Tst_000001",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            db.Users.Add(employee);
            db.SaveChanges();


            try
            {
                // Act
                var response = sysAdminController.RemoveManagerEmployeeRelationship(manager.Id, employee.Id);
                var response2 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == employee.Id);
                var response3 = db.Users.Include(x => x.Manages).FirstOrDefault(x => x.Id == manager.Id);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.NotNull(response2);
                Assert.Null(response3);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }

        }

    }
}
