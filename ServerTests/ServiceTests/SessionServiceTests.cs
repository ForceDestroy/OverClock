using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services;
using Server.Services.Interfaces;

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class SessionServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        public SessionServiceTests()
        {
        }

        [Fact]
        public async void SessionService_CreateSession_With_ExistingSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISessionService sessionService = new SessionService(db);

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
                SessionToken response = await sessionService.CreateSession(user);

                //Assert
                Assert.NotNull(response);
                Assert.Equal(1, db.SessionTokens.Count());
                Assert.NotEqual(token, response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }

        }

        [Fact]
        public async  void SessionService_CreateSession_With_No_ExistingSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISessionService sessionService = new SessionService(db);

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
                SessionToken response = await sessionService.CreateSession(user);

                //Assert
                Assert.NotNull(response);
                Assert.Equal(1, db.SessionTokens.Count());
                Assert.Equal(user, response.User);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }

        }

        [Fact]
        public async void SessionService_EndSession_With_MatchingSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISessionService sessionService = new SessionService(db);

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
                bool response = await sessionService.EndSession("TESTTOKEN");

                //Assert
                Assert.True(response);
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
        public async void SessionService_EndSession_With_No_MatchingSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISessionService sessionService = new SessionService(db);

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
                bool response = await sessionService.EndSession("WRONGTOKEN");

                //Assert
                Assert.False(response);
                Assert.Equal(1, db.SessionTokens.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void SessionService_GetSession_With_MatchingSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISessionService sessionService = new SessionService(db);

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
                SessionToken? response = await sessionService.GetSession("TESTTOKEN");

                //Assert
                Assert.NotNull(response);
                Assert.Equal(1, db.SessionTokens.Count());
                Assert.Equal(token, response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void SessionService_GetSession_With_No_MatchingSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ISessionService sessionService = new SessionService(db);

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
                SessionToken? response = await sessionService.GetSession("WRONGTOKEN");

                //Assert
                Assert.Null(response);
                Assert.Equal(1, db.SessionTokens.Count());
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
