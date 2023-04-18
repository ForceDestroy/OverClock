using Server.Controllers;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Server.BOModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]

    public class BulletinControllerTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public BulletinControllerTests()
        {

        }
        #region CreateAnnouncement
        [Fact]
        public async void BulletinController_CreateAnnouncement_When_SessionExists()
        {
            // Initialize
            DatabaseContext db = new(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
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
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };


            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcement = announcement, sessionToken = token.Token }));
            try
            {
                // Act
                var response = await bulletinController.CreateAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Single(db.Announcements);
                var a = await db.Announcements.FirstAsync();
                Assert.Equal(announcement.Title, a.Title);
                Assert.Equal(announcement.Body, a.Body);
                Assert.Equal(announcement.Date, a.Date);
                Assert.Equal(user, a.User);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_CreateAnnouncement_When_WrongAccessLevel()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
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
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };


            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcement = announcement, sessionToken = token.Token }));
            try
            {
                // Act
                var response = await bulletinController.CreateAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_CreateAnnouncement_When_NoSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcement = announcement, sessionToken = "NoToken" }));
            try
            {
                // Act
                var response = await bulletinController.CreateAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
                Assert.Empty(db.Announcements);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_CreateAnnouncement_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcement = announcement }));
            try
            {
                // Act
                var response = await bulletinController.CreateAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Empty(db.Announcements);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_CreateAnnouncement_When_IncorrectAnnouncementSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcement = "asda" , sessionToken = "token"}));
            try
            {
                // Act
                var response = await bulletinController.CreateAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Empty(db.Announcements);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetMyAnnouncements
        [Fact]
        public async void BulletinController_GetMyAnnouncements_When_SessionExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            User user = new User()
            {
                Id = "Tst_000005",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle",
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };
           
            User user1 = new User()
            {
                Id = "Tst_000006",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle",
                AccessLevel = 1
            };

            Announcement announcement = new Announcement()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            Announcement announcement1 = new Announcement()
            {
                User = user,
                Date = new DateTime(2022, 11, 27, 00, 00, 00),
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            Announcement announcement2 = new Announcement()
            {
                User = user1,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            db.Add(user);
            db.Add(user1);
            db.Add(announcement);
            db.Add(announcement1);
            db.Add(announcement2);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { sessionToken = token.Token }));
            try
            {
                // Act
                var response = await bulletinController.GetMyAnnouncements(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonSerializer.Deserialize<List<AnnouncementInfo>>(un);

                Assert.Equal(2, responseValue.Count);
                Assert.Equal(announcement.Date, responseValue.ElementAt(1).Date);
                Assert.Equal(announcement1.Date, responseValue.ElementAt(0).Date);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_GetMyAnnouncements_When_NoSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { sessionToken = "NoToken" }));
            try
            {
                // Act
                var response = await bulletinController.GetMyAnnouncements(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_GetMyAnnouncements_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { randomField= "ds" }));
            try
            {
                // Act
                var response = await bulletinController.GetMyAnnouncements(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetAllAnnouncements
        [Fact]
        public async void BulletinController_GetAllAnnouncements_When_SessionExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            User user = new User()
            {
                Id = "Tst_000005",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle",
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            User user1 = new User()
            {
                Id = "Tst_000006",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle",
                AccessLevel = 1
            };

            Announcement announcement = new Announcement()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            Announcement announcement1 = new Announcement()
            {
                User = user,
                Date = new DateTime(2022, 11, 27, 00, 00, 00),
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            Announcement announcement2 = new Announcement()
            {
                User = user1,
                Date = new DateTime(2022, 11, 28, 00, 00, 00),
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            db.Add(user);
            db.Add(user1);
            db.Add(announcement);
            db.Add(announcement1);
            db.Add(announcement2);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { sessionToken = token.Token }));
            try
            {
                // Act
                var response = await bulletinController.GetAllAnnouncements(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonSerializer.Deserialize<List<AnnouncementInfo>>(un);

                Assert.Equal(3, responseValue.Count);
                Assert.Equal(announcement.Date, responseValue.ElementAt(2).Date);
                Assert.Equal(announcement1.Date, responseValue.ElementAt(1).Date);
                Assert.Equal(announcement2.Date, responseValue.ElementAt(0).Date);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_GetAllAnnouncements_When_NoSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { sessionToken = "NoToken" }));
            try
            {
                // Act
                var response = await bulletinController.GetAllAnnouncements(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_GetAllAnnouncements_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { randomField = "ds" }));
            try
            {
                // Act
                var response = await bulletinController.GetMyAnnouncements(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region DeleteAnnouncement
        [Fact]
        public async void BulletinController_DeleteAnnouncement_When_SessionExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
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
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            Announcement announcement = new Announcement()
            {
                User = user,
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };


            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.Announcements.Add(announcement); 
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcementId = announcement.Id, sessionToken = token.Token }));
            try
            {
                // Act
                var response = await bulletinController.DeleteAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Empty(db.Announcements);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        public async void BulletinController_DeleteAnnouncement_When_NotManager()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
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
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 1
            };

            User user1 = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user1,
            };

            Announcement announcement = new()
            {
                User = user,
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };


            db.Users.Add(user);
            db.Users.Add(user1);
            db.SessionTokens.Add(token);
            db.Announcements.Add(announcement);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcementId = announcement.Id, sessionToken = token.Token }));
            try
            {
                // Act
                var response = await bulletinController.DeleteAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<UnauthorizedResult>(response);
                Assert.Single(db.Announcements);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_DeleteAnnouncement_When_NoSession()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange

            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcementId = 1, sessionToken = "NoToken" }));
            try
            {
                // Act
                var response = await bulletinController.DeleteAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void BulletinController_DeleteAnnouncement_When_IncorrectSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IEmployerService employerService = new EmployerService(db);
            ISessionService sessionService = new SessionService(db);
            IBulletinService bulletinService = new BulletinService(db);
            BulletinController bulletinController = new BulletinController(sessionService, bulletinService);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            // Arrange
            var data = EncryptionHelper.EncryptTLS(JsonSerializer.Serialize(new { announcement = "" }));
            try
            {
                // Act
                var response = await bulletinController.DeleteAnnouncement(new EncryptedData(data));


                // Assert 
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Empty(db.Announcements);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Announcements");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion
    }
}
