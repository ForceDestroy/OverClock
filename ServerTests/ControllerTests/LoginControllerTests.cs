using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using System.Text.Json;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class LoginControllerTests
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        public LoginControllerTests()
        {

        }
        #region Obsolete
        #region ValidateLoginObsolete
        [Fact]
        public async void LoginController_ValidateLoginObsolete_With_NoMatchingUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
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
                //Act
                var response = await loginController.ValidateLoginObsolete("wrongemail@gmail.com", "password");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void LoginController_ValidateLoginObsolete_With_InvalidPassword()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
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
                //Act
                var response = await loginController.ValidateLoginObsolete("test@gmail.com", "wrongpassword");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }

        }

        [Fact]
        public async void LoginController_ValidateLoginObsolete_With_CorrectPassword()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
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
                //Act
                var response = await loginController.ValidateLoginObsolete("test@gmail.com", "password");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkObjectResult>(response);
                Assert.Equal(1, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion
        #region LogoutObsolete

        [Fact]
        public async void LoginController_LogoutObsolete_With_ValidSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
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
                //Act
                var response = await loginController.LogoutObsolete(token.Token);

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(0, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }


        [Fact]
        public async void LoginController_LogoutObsolete_With_InvalidSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
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
                //Act
                var response = await loginController.LogoutObsolete("WrongToken");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Equal(1, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        #endregion

        #endregion

        #region ValidateLogin
        public async void LoginController_ValidateLogin_With_WrongDataScheme()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { wrongField = "wrongdata" }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                //Act
                var response = await loginController.ValidateLogin(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void LoginController_ValidateLogin_With_NoMatchingUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize( new LoginInfo() { Password = user.Password, Email = "wrongemail"}));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                //Act
                var response = await loginController.ValidateLogin(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void LoginController_ValidateLoginSecure_With_InvalidPassword()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel =1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new LoginInfo() { Password = "wrongpassword", Email = user.Email }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                //Act
                var response = await loginController.ValidateLogin(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }

        }

        [Fact]
        public async void LoginController_ValidateLoginSecure_With_CorrectPassword()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new LoginInfo() { Password = "password", Email = user.Email }));

            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                //Act
                var response = await loginController.ValidateLogin(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<string>(response.Value);
                Assert.Equal(1, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        #endregion

        #region Logout
        [Fact]
        public async void LoginController_Logout_With_WrongDataScheme()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { wrongField = "wrongdata" }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                //Act
                var response = await loginController.Logout(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void LoginController_Logout_With_ValidSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { token = token.Token }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                //Act
                var response = await loginController.Logout(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(0, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }


        [Fact]
        public async void LoginController_Logout_With_InvalidSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);
            ISessionService sessionService = new SessionService(db);
            LoginController loginController = new LoginController(loginService, sessionService);

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { token = "wrongtoken" }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();
            try
            {
                //Act
                var response = await loginController.Logout(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Equal(1, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion
    }
}
