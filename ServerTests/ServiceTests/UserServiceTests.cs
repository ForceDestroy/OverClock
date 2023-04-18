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
    public class UserServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");


        public UserServiceTests()
        {

        }

        [Fact]
        public async void UserService_UpdateAccount_When_UserExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password")
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle"
            };

            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userService.UpdateAccount(updatedUser);

                // Assert
                Assert.NotNull(response);
                Assert.NotEqual(userEmail, response.Email);
                Assert.Equal(userId, response.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }


        [Fact]
        public async void UserService_UpdateAccount_When_EmailExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password")
            };

            User otherUser = new User()
            {
                Id = "Tst_000001",
                Name = "Other User",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = otherUser.Email,
                Password = "A car is a vehicle"
            };

            db.Users.Add(otherUser);
            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userService.UpdateAccount(updatedUser);

                // Assert
                Assert.Null(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }






        [Fact]
        public async void UserService_UpdateAccount_When_UserDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle")
            };

            try
            {
                // Act
                var response = await userService.UpdateAccount(updatedUser);


                // Assert
                Assert.Null(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }


        }

        [Fact]
        public async void UserService_UpdateTheme_When_UserExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                ThemeColor = 0
            };

            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userService.UpdateTheme(userId, 1);


                // Assert
                Assert.NotNull(response);
                Assert.Equal(1, response.ThemeColor);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void UserService_UpdateTheme_When_UserDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

        
            try
            {
                // Act
                var response = await userService.UpdateTheme(userId, 1);


                // Assert
                Assert.Null(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }


        }

        [Fact]
        public async void UserService_GetAccountInfo_When_UserDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(user);
            db.SaveChanges();

            try
            {
                // Act
                var response = await userService.GetAccountInfo(userId);


                // Assert
                Assert.NotNull(response);
                Assert.Equal(userEmail, response.Email);
                Assert.Equal(userId, response.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }


        }

        [Fact]
        public async void UserService_GetAccountInfo_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";
            User user = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };


            try
            {
                // Act
                var response = await userService.GetAccountInfo(userId);


                // Assert
                Assert.Null(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");

            }


        }



    }
}
