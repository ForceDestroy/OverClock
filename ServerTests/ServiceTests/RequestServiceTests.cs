using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using Server.Enums;

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class RequestServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public RequestServiceTests()
        {

        }

        #region RequestTimeOff
        [Fact]
        public async void RequestService_RequestTimeOff_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestService.RequestTimeOff(requestInfo, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_RequestTimeOff_When_UserDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            db.Users.Add(employer);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestService.RequestTimeOff(requestInfo, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        [Fact]
        public async void RequestService_RequestTimeOff_When_EmployerDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                AccessLevel = 0
            };


            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestService.RequestTimeOff(requestInfo, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region RequestMultipleTimesOff
        [Fact]
        public async void RequestService_RequestMultipleTimesOff_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            DateTime date = DateTime.Now;
            DateTime date2 = DateTime.Now.AddMonths(30);

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = date,
                StartTime = date,
                EndTime = date
            };

            RequestInfo requestInfo2 = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "My grandma is visiting",
                Date = date2,
                StartTime = date2,
                EndTime = date2
            };

            ICollection<RequestInfo> requestInfos = new List<RequestInfo>();
            requestInfos.Add(requestInfo);
            requestInfos.Add(requestInfo2);

            try
            {
                // Act
                var response = await requestService.RequestMultipleTimesOff(requestInfos, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.Date.Equals(date));
                var response3 = db.Requests.FirstOrDefault(request => request.Date.Equals(date2));

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
                Assert.Equal(requestInfo.Body, response2.Body);
                Assert.NotNull(response3);
                Assert.Equal(employee.Id, response3.FromId);
                Assert.Equal(employer.Id, response3.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response3.Type);
                Assert.Equal(RequestStatus.Pending.ToString(), response3.Status);
                Assert.Equal(requestInfo2.Body, response3.Body);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_RequestMultipleTimesOff_When_UserDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            db.Users.Add(employer);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            RequestInfo requestInfo2 = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "My grandma is visiting",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            ICollection<RequestInfo> requestInfos = new List<RequestInfo>();
            requestInfos.Add(requestInfo);
            requestInfos.Add(requestInfo2);

            try
            {
                // Act
                var response = await requestService.RequestMultipleTimesOff(requestInfos, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        [Fact]
        public async void RequestService_RequestMultipleTimesOff_When_EmployerDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                AccessLevel = 0
            };


            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            RequestInfo requestInfo2 = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "My grandma is visiting",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            ICollection<RequestInfo> requestInfos = new List<RequestInfo>();
            requestInfos.Add(requestInfo);
            requestInfos.Add(requestInfo2);


            try
            {
                // Act
                var response = await requestService.RequestMultipleTimesOff(requestInfos, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion


        #region CustomRequest
        [Fact]
        public async void RequestService_CustomRequest_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request for my custom needs",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestService.CustomRequest(requestInfo, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_CustomRequest_When_UserDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            db.Users.Add(employer);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestService.CustomRequest(requestInfo, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_CustomRequest_When_EmployerDoesntExists()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                AccessLevel = 0
            };


            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestService.CustomRequest(requestInfo, userId);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region GetOwnRequests
        [Fact]
        public async void RequestService_GetOwnRequests_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

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

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.GetOwnRequests(userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotEmpty(response);
                Assert.Equal(response.First().FromId, userId);
                Assert.Equal(response.First().ToId, employer.Id);
                Assert.Equal(response.First().Body, request.Body);
                Assert.Equal(response.First().Type, RequestType.Personal.ToString());

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_GetOwnRequests_When_UserDoesntExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                Manager = employer
            };

            Request request = new Request()
            {
                FromId = employee.Id,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Status = RequestStatus.Pending.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.GetOwnRequests(userId);

                // Assert
                Assert.Empty(response);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region EmployerGetAllRequests
        [Fact]
        public async void RequestService_EmployerGetAllRequests_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = userId,
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

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

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.EmployerGetAllRequests(userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotEmpty(response);
                Assert.Equal(response.First().FromId, userId);
                Assert.Equal(response.First().ToId, employer.Id);
                Assert.Equal(response.First().Body, request.Body);
                Assert.Equal(response.First().Type, RequestType.Personal.ToString());

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_EmployerGetAllRequests_When_UserDosentExist()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange
            string userEmail = "test@gmail.com";
            string userId = "Tst_000000";

            User employer = new User()
            {
                Id = userId,
                Name = "Aiman Hanna",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("A car is a vehicle"),
                AccessLevel = 1
            };

            User employee = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer
            };

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

            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.EmployerGetAllRequests(userId);

                // Assert
                Assert.Empty(response);


            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        #endregion

        #region ApproveRequest
        [Fact]
        public async void RequestService_ApproveRequest_When_Approved()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>()
            };

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = RequestStatus.Pending.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            try
            {
                // Act
                var response = await requestService.ApproveRequest(request.Id, newStatus);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Approved.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApproveRequest_When_Denied()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>()
            };

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = RequestStatus.Pending.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Denied.ToString();

            try
            {
                // Act
                var response = await requestService.ApproveRequest(request.Id, newStatus);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Denied.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApproveRequest_When_Acknowledged()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>()
            };

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = RequestStatus.Pending.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Acknowledged.ToString();

            try
            {
                // Act
                var response = await requestService.ApproveRequest(request.Id, newStatus);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Acknowledged.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApproveRequest_When_WrongNewStatus()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>()
            };

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = RequestStatus.Pending.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = "random";

            try
            {
                // Act
                var response = await requestService.ApproveRequest(request.Id, newStatus);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.NotNull(response2);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApproveRequest_When_WrongRequestId()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>()
            };

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = RequestStatus.Pending.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            var random = new Random();
            int randomId = random.Next(1, 1000000);

            while (randomId == request.Id)
            {
                randomId = random.Next(1, 1000000);
            }

            try
            {
                // Act
                var response = await requestService.ApproveRequest(randomId, newStatus);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.NotNull(response2);
                Assert.Equal("Pending", response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApproveRequest_When_UnauthorizedAccess()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };

            User otherEmployer = new User()
            {
                Id = "Tst_000002",
                Name = "Sango",
                Email = "test3@gmail.com",
                Password = EncryptionHelper.EncryptString("A vehicle is not a car"),
                AccessLevel = 1,
                ReceivedRequests = new List<Request>()
            };

            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                SentRequests = new List<Request>(),
                Manager = otherEmployer
            };


            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a day off",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = RequestStatus.Pending.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Users.Add(otherEmployer);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            try
            {
                // Act
                var response = await requestService.ApproveRequest(request.Id, newStatus);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.False(response);
                Assert.NotNull(response2);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        #endregion

        #region ApprovedRequestLogic

        [Fact]
        public async void RequestService_ApprovedRequestLogic_When_VacationRequest()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Workhours");
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };
            int initialVacationDays = 10;
            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>(),
                VacationDays = initialVacationDays,
            };
            int dayGap = 5;
            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Vacation.ToString(),
                Body = "Its Christmas",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(dayGap),
                Status = RequestStatus.Approved.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.ApprovedRequestLogic(userId, request.Id);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);
                var response3 = db.Users.FirstOrDefault(user => user.Id == userId);
                var response4 = db.WorkHours.Include(x => x.User).Where(workhour => workhour.User.Id == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.NotNull(response4);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Vacation.ToString(), response2.Type);
                Assert.Equal(initialVacationDays - dayGap - 1, response3.VacationDays);
                Assert.Equal(dayGap + 1, response4.Count());
                Assert.Equal(9, response4.First().StartTime.Hour);
                Assert.Equal(17, response4.First().EndTime.Hour);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Workhours");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        [Fact]
        public async void RequestService_ApprovedRequestLogic_When_SickRequest()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Workhours");
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };
            int initialSickDays = 10;
            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>(),
                SickDays = initialSickDays,
            };
            int dayGap = 5;
            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Sick.ToString(),
                Body = "Its Christmas",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(dayGap),
                Status = RequestStatus.Approved.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.ApprovedRequestLogic(userId, request.Id);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);
                var response3 = db.Users.FirstOrDefault(user => user.Id == userId);
                var response4 = db.WorkHours.Include(x => x.User).Where(workhour => workhour.User.Id == userId);

                // Assert
                Assert.True(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.NotNull(response4);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Sick.ToString(), response2.Type);
                Assert.Equal(initialSickDays - dayGap - 1, response3.SickDays);
                Assert.Equal(dayGap + 1, response4.Count());
                Assert.Equal(9, response4.First().StartTime.Hour);
                Assert.Equal(17, response4.First().EndTime.Hour);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Workhours");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApprovedRequestLogic_When_NotEnoughSickDays()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Workhours");
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };
            int initialSickDays = 1;
            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>(),
                SickDays = initialSickDays,
            };
            int dayGap = 5;
            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Sick.ToString(),
                Body = "Its Christmas",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(dayGap),
                Status = RequestStatus.Approved.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.ApprovedRequestLogic(userId, request.Id);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);
                var response3 = db.Users.FirstOrDefault(user => user.Id == userId);
                var response4 = db.WorkHours.Include(x => x.User).Where(workhour => workhour.User.Id == userId);

                // Assert
                Assert.False(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.Empty(response4);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Sick.ToString(), response2.Type);
                Assert.Equal(initialSickDays, response3.SickDays);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Workhours");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        [Fact]
        public async void RequestService_ApprovedRequestLogic_When_NotEnoughVacationDays()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Workhours");
            db.Database.ExecuteSqlRaw("delete from Requests");
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
                ReceivedRequests = new List<Request>()
            };
            int initialVacationDays = 1;
            User employee = new User()
            {
                Id = userId,
                Name = "John Smith",
                Email = userEmail,
                Password = EncryptionHelper.EncryptString("password"),
                AccessLevel = 0,
                Manager = employer,
                SentRequests = new List<Request>(),
                VacationDays = initialVacationDays,
            };
            int dayGap = 5;
            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Vacation.ToString(),
                Body = "Its Christmas",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(dayGap),
                Status = RequestStatus.Approved.ToString()
            };

            Request request = new Request(requestInfo);
            employer.ReceivedRequests.Add(request);
            employee.SentRequests.Add(request);

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestService.ApprovedRequestLogic(userId, request.Id);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);
                var response3 = db.Users.FirstOrDefault(user => user.Id == userId);
                var response4 = db.WorkHours.Include(x => x.User).Where(workhour => workhour.User.Id == userId);

                // Assert
                Assert.False(response);
                Assert.NotNull(response2);
                Assert.NotNull(response3);
                Assert.Empty(response4);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Vacation.ToString(), response2.Type);
                Assert.Equal(initialVacationDays, response3.VacationDays);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Workhours");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestService_ApprovedRequestLogic_When_NoRequest()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            db.Database.ExecuteSqlRaw("delete from Workhours");
            db.Database.ExecuteSqlRaw("delete from Requests");
            db.Database.ExecuteSqlRaw("delete from Users");

            // Arrange


            try
            {
                // Act
                var response = await requestService.ApprovedRequestLogic("", 1);

                // Assert
                Assert.False(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from Workhours");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
            #endregion
        }
    }
}
