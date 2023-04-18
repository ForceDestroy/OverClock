using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Services.Interfaces;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class BulletinServiceTests:TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public BulletinServiceTests()
        {

        }

        #region CreateAnnouncement
        [Fact]
        public async void BulletinService_CreateAnnouncement_When_CorrectValues()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            User employer = new User()
            {
                Id = "Tst_000005",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle",
                AccessLevel = 1
            };

            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            db.Users.Add(employer);
            db.SaveChanges();

            try
            {
                // Act
                var response = await bulletinService.CreateAnnouncement(announcement, employer.Id);

                // Assert
                Assert.True(response);
                Assert.Single(db.Announcements);
                var a = await db.Announcements.FirstAsync();
                Assert.Equal(announcement.Title, a.Title);
                Assert.Equal(announcement.Body, a.Body);
                Assert.Equal(announcement.Date, a.Date);
                Assert.Equal(employer, a.User);
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
        public async void BulletinService_CreateAnnouncement_When_NoUser()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            User employer = new User()
            {
                Id = "Tst_000005",
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = "A car is a vehicle",
                AccessLevel = 1
            };

            AnnouncementInfo announcement = new AnnouncementInfo()
            {
                Date = DateTime.Now.Date,
                Title = "Day Off",
                Body = "Tomorrow will be a Day Off"
            };

            try
            {
                // Act
                var response = await bulletinService.CreateAnnouncement(announcement, employer.Id);

                // Assert
                Assert.False(response);
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
        public async void BulletinService_GetMyAnnouncements_When_CorrectValues()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
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
            db.SaveChanges();

            try
            {
                // Act
                var response = await bulletinService.GetMyAnnouncements(user.Id);

                // Assert
                Assert.Equal(2, response.Count());
                Assert.Equal(announcement.Date, response.ElementAt(1).Date);
                Assert.Equal(announcement1.Date, response.ElementAt(0).Date);
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
        public async void BulletinService_GetMyAnnouncements_When_NoUser()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange
            
            try
            {
                // Act
                var response = await bulletinService.GetMyAnnouncements("NoID");

                // Assert
                Assert.Empty(response);
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
        public async void BulletinService_GetAllAnnouncements_When_CorrectValues()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
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
            db.SaveChanges();

            try
            {
                // Act
                var response = await bulletinService.GetAllAnnouncements();

                // Assert
                Assert.Equal(3, response.Count());
                Assert.Equal(announcement.Date, response.ElementAt(2).Date);
                Assert.Equal(announcement1.Date, response.ElementAt(1).Date);
                Assert.Equal(announcement2.Date, response.ElementAt(0).Date);
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
        public async void BulletinService_GetGetAllAnnouncements_When_NoAnnouncements()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
            db.Database.ExecuteSqlRaw("delete from Announcements");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");


            // Arrange

            try
            {
                // Act
                var response = await bulletinService.GetAllAnnouncements();

                // Assert
                Assert.Empty(response);
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
        public async void BulletinService_DeleteAnnouncement_When_CorrectValues()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
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

            db.Add(user);
            db.Add(user1);
            db.Add(announcement);
            db.SaveChanges();

            try
            {
                // Act
                var response = await bulletinService.DeleteAnnouncement(announcement.Id);

                // Assert
                Assert.True(response);
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
        public async void BulletinService_DeleteAnnouncement_When_NoAnnouncement()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IBulletinService bulletinService = new BulletinService(db);
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

            db.Add(user);
            db.Add(user1);
            db.Add(announcement);
            db.SaveChanges();

            try
            {
                // Act
                var response = await bulletinService.DeleteAnnouncement(2);

                // Assert
                Assert.False(response);
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
        #endregion
    }
}
