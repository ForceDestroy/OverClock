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

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class LoginServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        public LoginServiceTests()
        {
        }

        [Fact]
        public async void LoginService_GetUser_With_MatchingUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);

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
                User response = await loginService.GetUser(user.Email);

                //Assert
                Assert.NotNull(response);
                Assert.Equal(user, response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }

        }

        [Fact]
        public async void LoginService_GetUser_With_No_MatchingUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);

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
                User response = await loginService.GetUser("wrongaddress@gmail.com");

                //Assert
                Assert.Null(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }

        }

        [Fact]
        public async void LoginService_ValidateLogin_With_CorrectPassword()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);

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
                bool response = await loginService.ValidateLogin("test@gmail.com", "password");

                //Assert
                Assert.True(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }

        }

        [Fact]
        public async void LoginService_ValidateLogin_With_WrongPassword()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ILoginService loginService = new LoginService(db);

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
                bool response = await loginService.ValidateLogin("test@gmail.com", "wrongpassword");

                //Assert
                Assert.False(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
            }

        }
    }
}
