using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class UserControllerTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
      
        public UserControllerTests()
        {
               
        }
        #region Obsolete

        [Fact]
        public async void UserController_UpdateAccountObsolete_When_CorrectUser()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password ="A car is a vehicle"
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userController.UpdateAccountObsolete(updatedUser, token.Token);


                // Assert
                Assert.NotNull(response.Value);
                Assert.NotEqual(userEmail, response.Value.Email);
                Assert.Equal(userId, response.Value.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }


        }

        [Fact]
        public async void UserController_UpdateAccountObsolete_When_UserSessionDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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
                Password = EncryptionHelper.EncryptString("A car is a vehicle")
            };

            string randomToken = "random token";

            try
            {
                // Act
                var response = await userController.UpdateAccountObsolete(updatedUser, randomToken);


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
        public async void UserController_UpdateAccountObsolete_When_DifferentUserUpdating()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            User randomUser = new User()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user")
            };
            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = randomUser,
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle")
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userController.UpdateAccountObsolete(updatedUser, token.Token);


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
        public async void UserController_UpdateThemeObsolete_When_SessionExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userController.UpdateThemeObsolete(1, token.Token);


                // Assert
                Assert.NotNull(response.Value);
                Assert.Equal(1, response.Value.ThemeColor);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void UserController_UpdateThemeObsolete_When_SessionDoesNotExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userController.UpdateThemeObsolete(1, "SomeExpiredToken");


                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void UserService_GetAccountInfoObsolete_When_UserDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                // Act
                var response = await userController.GetAccountInfoObsolete(token.Token);


                // Assert
                Assert.NotNull(response);
                Assert.Equal(userEmail, response.Value.Email);
                Assert.Equal(userId, response.Value.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }


        }

        [Fact]
        public async void UserService_GetAccountInfoObsolete_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            User randomUser = new User()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user")
            };

            string randomToken = "randomToken";

            try
            {
                // Act
                var response = await userController.GetAccountInfoObsolete(randomToken);


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
        #endregion

        #region UpdateAccount
        [Fact]
        public async void UserController_UpdateAccount_When_CorrectUser()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle"
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { newUser = updatedUser, sessionToken = token.Token }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userController.UpdateAccount(new EncryptedData(data));


                // Assert
                Assert.NotNull(response.Value);
                Assert.NotEqual(userEmail, user.Email);
                Assert.Equal(userId, user.Id);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }


        }

        [Fact]
        public async void UserController_UpdateAccount_When_UserSessionDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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
                Password = EncryptionHelper.EncryptString("A car is a vehicle")
            };

            string randomToken = "random token";

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { newUser = updatedUser, sessionToken = randomToken }));

            try
            {
                // Act
                var response = await userController.UpdateAccount(new EncryptedData(data));


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
        public async void UserController_UpdateAccount_When_DifferentUserUpdating()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            User randomUser = new User()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user")
            };
            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = randomUser,
            };

            UserProfile updatedUser = new UserProfile()
            {
                Id = "Tst_000000",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle")
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { newUser = updatedUser, sessionToken = token.Token }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                // Act
                var response = await userController.UpdateAccount(new EncryptedData(data));


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
        public async void UserController_UpdateAccount_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { randomField = "RandomValue" }));

            try
            {
                // Act
                var response = await userController.UpdateAccount(new EncryptedData(data));


                // Assert
                Assert.Null(response.Value);
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
        public async void UserController_UpdateAccount_When_IncorrectUserSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { newUser = "RandomValue", sessionToken = "token" }));

            try
            {
                // Act
                var response = await userController.UpdateAccount(new EncryptedData(data));


                // Assert
                Assert.Null(response.Value);
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

        #region GetAccountInfo
        [Fact]
        public async void UserService_GetAccountInfo_When_UserDoesExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await userController.GetAccountInfo(new EncryptedData(data));


                // Assert
                Assert.NotNull(response);
                Assert.IsType<string>(response.Value);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");

            }
        }

        [Fact]
        public async void UserService_GetAccountInfo_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            User randomUser = new User()
            {
                Id = "Tst_000005",
                Name = "Random User",
                Email = "testRandom@gmail.com",
                Password = EncryptionHelper.EncryptString("the random user")
            };

            string randomToken = "randomToken";

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { sessionToken = randomToken }));

            try
            {
                // Act
                var response = await userController.GetAccountInfo(new EncryptedData(data));


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
        public async void UserService_GetAccountInfo_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { randomField = token.Token }));

            try
            {
                // Act
                var response = await userController.GetAccountInfo(new EncryptedData(data));


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

        #region UpdateTheme
        [Fact]
        public async void UserController_UpdateTheme_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { randomField = 1 }));
            try
            {
                // Act
                var response = await userController.UpdateTheme(new EncryptedData(data));


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
        public async void UserController_UpdateTheme_When_SessionExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { themeColor = 1, sessionToken = token.Token }));
            try
            {
                // Act
                var response = await userController.UpdateTheme(new EncryptedData(data));


                // Assert
                Assert.NotNull(response.Value);
                Assert.Equal(1, user.ThemeColor);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void UserController_UpdateTheme_When_SessionDoesNotExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IUserService userService = new UserService(db);
            ISessionService sessionService = new SessionService(db);
            UserController userController = new UserController(userService, sessionService);
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            db.Users.Add(user);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { themeColor = 1, sessionToken = "expiredToken" }));
            try
            {
                // Act
                var response = await userController.UpdateTheme(new EncryptedData(data));


                // Assert
                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion
    }
}
