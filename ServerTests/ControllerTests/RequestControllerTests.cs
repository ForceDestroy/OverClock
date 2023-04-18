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
using Server.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class RequestControllerTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public RequestControllerTests()
        {
            
        }
        #region Obsolete
        #region RequestTimeOffObsolete
        [Fact]
        public async void RequestController_RequestTimeOffObsolete_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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
                var response = await requestController.RequestTimeOffObsolete(requestInfo, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestTimeOffObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
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
                var response = await requestController.RequestTimeOffObsolete(requestInfo, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestTimeOffObsolete_When_RequestIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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
                var response = await requestController.RequestTimeOffObsolete(requestInfo, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Null(response2);
                
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region RequestMultipleTimesOffObsolete
        [Fact]
        public async void RequestController_RequestMultipleTimesOffObsolete_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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
                var response = await requestController.RequestMultipleTimesOffObsolete(requestInfos, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.Date.Equals(date));
                var response3 = db.Requests.FirstOrDefault(request => request.Date.Equals(date2));

                // Assert
                Assert.NotNull(response);
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
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestMultipleTimesOffObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
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
                var response = await requestController.RequestMultipleTimesOffObsolete(requestInfos, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestMultipleTimesOffObsolete_When_RequestIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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
                var response = await requestController.RequestMultipleTimesOffObsolete(requestInfos, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region CustomRequestObsolete 

        [Fact]
        public async void RequestController_CustomRequestObsolete_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestController.CustomRequestObsolete(requestInfo, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_CustomRequestObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestController.CustomRequestObsolete(requestInfo, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_CustomRequestObsolete_When_RequestIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            try
            {
                // Act
                var response = await requestController.CustomRequestObsolete(requestInfo, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region GetOwnRequestsObsolete
        [Fact]
        public async void RequestController_GetOwnRequestsObsolete_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestController.GetOwnRequestsObsolete(token.Token);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response.Value);
                Assert.NotEmpty(response.Value);
                Assert.Equal(response.Value.First().FromId, userId);
                Assert.Equal("John Smith", response.Value.First().FromName);
                Assert.Equal(response.Value.First().ToId, employer.Id);
                Assert.Equal("Aiman Hanna",response.Value.First().ToName);
                Assert.Equal(response.Value.First().Body, request.Body);
                Assert.Equal(response.Value.First().Type, RequestType.Personal.ToString());
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_GetOwnRequestsObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            string randomToken = "RandomTOken";

            try
            {
                // Act
                var response = await requestController.GetOwnRequestsObsolete(randomToken);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region EmployerGetAllRequestsObsolete

        [Fact]
        public async void RequestController_EmployerGetAllRequestsObsolete_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            try
            {
                // Act
                var response = await requestController.EmployerGetAllRequestsObsolete(token.Token);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response.Value);
                Assert.NotEmpty(response.Value);
                Assert.Equal(response.Value.First().FromId, userId);
                Assert.Equal(response.Value.First().ToId, employer.Id);
                Assert.Equal(response.Value.First().Body, request.Body);
                Assert.Equal(response.Value.First().Type, RequestType.Personal.ToString());
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_EmployerGetAllRequestsObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            string randomToken = "RandomTOken";

            try
            {
                // Act
                var response = await requestController.EmployerGetAllRequestsObsolete(randomToken);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        #endregion

        #region ApproveRequestObsolete
        [Fact]
        public async void RequestController_ApproveRequestObsolete_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            try
            {
                // Act
                var response = await requestController.ApproveRequestObsolete(request.Id, newStatus, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Approved.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_ApproveRequestObsolete_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            string randomToken = "RANDOMTOKEN";

            try
            {
                // Act
                var response = await requestController.ApproveRequestObsolete(request.Id, newStatus, randomToken);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.NotNull(response2);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_ApproveRequestObsolete_When_InvalidRequest()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = "";

            try
            {
                // Act
                var response = await requestController.ApproveRequestObsolete(request.Id, newStatus, token.Token);
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.NotNull(response2);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #endregion

        #region RequestTimeOff
        [Fact]
        public async void RequestController_RequestTimeOff_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = requestInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestTimeOff(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestTimeOff_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = requestInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestTimeOff(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }


        [Fact]
        public async void RequestController_RequestTimeOff_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "RandomValue" }));

            try
            {
                // Act
                var response = await requestController.RequestTimeOff(new EncryptedData(data));
                

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestTimeOff_When_RequestIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = requestInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestTimeOff(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestTimeOff_When_InvalidRequestSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = "notRequestInfo", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestTimeOff(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region RequestMultipleTimesOff
        [Fact]
        public async void RequestController_RequestMultipleTimesOff_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { listOfRequests = requestInfos, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestMultipleTimesOff(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.Date.Equals(date));
                var response3 = db.Requests.FirstOrDefault(request => request.Date.Equals(date2));

                // Assert
                Assert.NotNull(response);
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
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestMultipleTimesOff_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { listOfRequests = requestInfos, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestMultipleTimesOff(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestMultipleTimesOff_When_RequestIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { listOfRequests = requestInfos, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestMultipleTimesOff(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestMultipleTimesOff_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomField" }));

            try
            {
                // Act
                var response = await requestController.RequestMultipleTimesOff(new EncryptedData(data));
                
                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_RequestMultipleTimesOff_When_InvalidListOfRequestSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
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

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { listOfRequests = "randomField", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.RequestMultipleTimesOff(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region CustomRequest 

        [Fact]
        public async void RequestController_CustomRequest_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = requestInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.CustomRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_CustomRequest_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.Users.Add(employer);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = employer.Id,
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = requestInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.CustomRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
                Assert.Null(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_CustomRequest_When_RequestIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = requestInfo, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.CustomRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_CustomRequest_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomField" }));

            try
            {
                // Act
                var response = await requestController.CustomRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_CustomRequest_When_InvalidRequestSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employee);
            db.SaveChanges();

            RequestInfo requestInfo = new RequestInfo()
            {
                FromId = userId,
                ToId = "",
                Type = RequestType.Personal.ToString(),
                Body = "I need a custom request",
                Date = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestInfo = "randomField", sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.CustomRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Null(response2);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region GetOwnRequests
        [Fact]
        public async void RequestController_GetOwnRequests_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.GetOwnRequests(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<RequestInfo>>(un);
                Assert.NotNull(responseValue);
                Assert.NotEmpty(responseValue);
                Assert.Equal(responseValue.First().FromId, userId);
                Assert.Equal("John Smith", responseValue.First().FromName);
                Assert.Equal(responseValue.First().ToId, employer.Id);
                Assert.Equal("Aiman Hanna", responseValue.First().ToName);
                Assert.Equal(responseValue.First().Body, request.Body);
                Assert.Equal(responseValue.First().Type, RequestType.Personal.ToString());
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_GetOwnRequests_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            string randomToken = "RandomTOken";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = randomToken }));

            try
            {
                // Act
                var response = await requestController.GetOwnRequests(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_GetOwnRequests_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomField" }));

            try
            {
                // Act
                var response = await requestController.GetOwnRequests(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region EmployerGetAllRequests

        [Fact]
        public async void RequestController_EmployerGetAllRequests_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.EmployerGetAllRequests(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<ICollection<RequestInfo>>(un);
                Assert.NotNull(responseValue);
                Assert.NotEmpty(responseValue);
                Assert.Equal(responseValue.First().FromId, userId);
                Assert.Equal(responseValue.First().ToId, employer.Id);
                Assert.Equal(responseValue.First().Body, request.Body);
                Assert.Equal(responseValue.First().Type, RequestType.Personal.ToString());
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_EmployerGetAllRequests_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employee,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            string randomToken = "RandomTOken";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = randomToken }));

            try
            {
                // Act
                var response = await requestController.EmployerGetAllRequests(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);

            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_EmployerGetAllRequests_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomField" }));

            try
            {
                // Act
                var response = await requestController.EmployerGetAllRequests(new EncryptedData(data));

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

        #region ApproveRequest
        [Fact]
        public async void RequestController_ApproveRequest_When_Valid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestId = request.Id, newStatus = newStatus, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.ApproveRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response2);
                Assert.Equal(employee.Id, response2.FromId);
                Assert.Equal(employer.Id, response2.ToId);
                Assert.Equal(RequestType.Personal.ToString(), response2.Type);
                Assert.Equal(RequestStatus.Approved.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_ApproveRequest_When_SessionIsInvalid()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            string randomToken = "RANDOMTOKEN";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestId = request.Id, newStatus = newStatus, sessionToken = randomToken }));

            try
            {
                // Act
                var response = await requestController.ApproveRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.NotNull(response2);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_ApproveRequest_When_InvalidRequest()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();

            string newStatus = "";

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { requestId = request.Id, newStatus = newStatus, sessionToken = token.Token }));

            try
            {
                // Act
                var response = await requestController.ApproveRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestResult>(response);
                Assert.NotNull(response2);
                Assert.Equal(RequestStatus.Pending.ToString(), response2.Status);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        [Fact]
        public async void RequestController_ApproveRequest_When_InvalidSchema()
        {
            // Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IRequestService requestService = new RequestService(db);
            ISessionService sessionService = new SessionService(db);
            RequestController requestController = new RequestController(requestService, sessionService);
            db.Database.ExecuteSqlRaw("delete from SessionTokens");
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

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = employer,
            };

            db.SessionTokens.Add(token);
            db.Users.Add(employer);
            db.Users.Add(employee);
            db.Requests.Add(request);
            db.SaveChanges();


            string newStatus = RequestStatus.Approved.ToString();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "randomField" }));

            try
            {
                // Act
                var response = await requestController.ApproveRequest(new EncryptedData(data));
                var response2 = db.Requests.FirstOrDefault(request => request.FromId == userId);

                // Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.NotNull(response2);
            }
            finally
            {
                // Dispose
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
                db.Database.ExecuteSqlRaw("delete from Requests");
                db.Database.ExecuteSqlRaw("delete from Users");

            }
        }

        #endregion

    }
}
