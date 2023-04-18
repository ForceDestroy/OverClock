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
using Server.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]
    public class TimeSheetControllerTests
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        public TimeSheetControllerTests()
        {

        }
        #region Obsolete

        #region LogHours
        [Fact]
        public async void TimeSheetController_LogHoursObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00)
            });

            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                //Act
                var response = await timeSheetController.LogHoursObsolete(submission, "NonexistentSession");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHoursObsolete_With_InvalidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2021, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00)
            });

            try
            {
                //Act
                var response = await timeSheetController.LogHoursObsolete(submission, "TESTTOKEN");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHoursObsolete_With_ExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
                Id = 0

            };

            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00).ToLocalTime()
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00).ToLocalTime()
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00).ToLocalTime()
            });

            try
            {
                //Act
                var response = await timeSheetController.LogHoursObsolete(submission, "TESTTOKEN");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHoursObsolete_With_ValidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00)
            });

            try
            {
                //Act
                var response = await timeSheetController.LogHoursObsolete(submission, "TESTTOKEN");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(3, db.WorkHours.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetTimeSheet
        [Fact]
        public async void TimeSheetController_GetTimeSheetObsolete_With_ValidSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 10, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 12, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetController.GetTimeSheetObsolete(workHours.StartTime, token.Token );

                //Assert
                Assert.NotNull(response);
                Assert.Equal(7, response.Value.Count());
                Assert.Equal(2, response.Value.ToList()[0].Count());
                Assert.Single(response.Value.ToList()[2]);
                Assert.Single(response.Value.ToList()[6]);
                Assert.Equal(workHours3.StartTime, response.Value.ToList()[6].First().StartTime);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetTimeSheetObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange

            try
            {
                //Act
                var response = await timeSheetController.GetTimeSheetObsolete(DateTime.Now, "NonexistentSession");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region SubmitTimeSheet
        [Fact]
        public async void TimeSheetController_SubmitTimeSheetObsolete_With_ApprovedSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 20, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 20, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 21, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 21, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 23, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 26, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 18, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheetObsolete( token.Token, submission);

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_SubmitTimeSheetObsolete_With_ValidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 23, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 18, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheetObsolete(token.Token, submission);

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(4, db.WorkHours.Count());
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 20, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 21, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 23, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)));
                Assert.Equal(db.WorkHours.First(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)).StartTime, new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime());

            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_SubmitTimeSheetObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheetObsolete( "NonexistentSession",submission);

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetHours
        [Fact]
        public async void TimeSheetController_GetHoursObsolete_With_ExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService,payslipService);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetController.GetHoursObsolete(new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(), token.Token );

                //Assert
                Assert.NotNull(response);
                Assert.Equal(2, response.Value.Count());
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetHoursObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 15, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 17, 00, 00),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetController.GetHoursObsolete(new DateTime(2020, 01, 01, 00, 00, 00), "InexistentSession");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetSubmittedTimeSheets
        [Fact]
        public async void TimeSheetController_GetSubmittedTimeSheetsObsolete_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = manager,
            };

            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            User user1 = new User()
            {
                Id = "Tst_000002",
                Name = "John Smith",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 17, 30, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 23, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Sick.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours4 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours5 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours6 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours7 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Holiday.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours8 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 28, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 28, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 28, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            db.Users.Add(manager);
            db.Users.Add(user);
            db.Users.Add(user1);
            db.SessionTokens.Add(token);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.WorkHours.Add(workHours4);
            db.WorkHours.Add(workHours5);
            db.WorkHours.Add(workHours6);
            db.WorkHours.Add(workHours7);
            db.WorkHours.Add(workHours8);
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetController.GetSubmittedTimeSheetsObsolete(token.Token);

                //Assert
                Assert.NotNull(response.Value);
                Assert.Equal(3, response.Value.Count());
                Assert.Equal(28.5, response.Value.First(x => x.UserId == user.Id).HoursWorked);
                Assert.Equal(8, response.Value.First(x => x.UserId == user.Id).PaidSick);
                Assert.Equal(8, response.Value.First(x => x.UserId == user1.Id).PaidHoliday);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetSubmittedTimeSheetsObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(manager);
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetController.GetSubmittedTimeSheetsObsolete("token");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result) ;
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region ApproveTimeSheet
        [Fact]
        public async void TimeSheetService_ApproveTimeSheetObsolete_With_NoSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };
            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = manager,
            };
            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };


            db.Users.Add(manager);
            db.Users.Add(user);
            db.Add(token);
            db.SaveChanges();


            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheetObsolete(token.Token, new TimeSheetSummary());

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Equal(0, db.Payslips.Count());

            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetService_ApproveTimeSheetObsolete_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = manager,
            };

            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 17, 30, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 23, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Sick.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours4 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours5 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 28, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 28, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 28, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            List<WorkHours> hours = new List<WorkHours>();
            hours.Add(workHours);
            hours.Add(workHours1);
            hours.Add(workHours2);
            hours.Add(workHours3);
            hours.Add(workHours4); 
            hours.Add(workHours5);

            TimeSheetSummary timeSheet = new TimeSheetSummary(hours,user);

            db.Users.Add(manager);
            db.Users.Add(user);
            db.Add(token);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.WorkHours.Add(workHours4);
            db.WorkHours.Add(workHours5);
            db.SaveChanges();


            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheetObsolete(token.Token, timeSheet);

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours1.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours2.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours3.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours4.Status);
                Assert.Equal(WorkHourStatus.Submitted.ToString(), workHours5.Status);
                Assert.Equal(1, db.Payslips.Count());


            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetService_ApproveTimeSheetObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            List<WorkHours> hours = new List<WorkHours>();
            hours.Add(workHours);

            TimeSheetSummary timeSheet = new TimeSheetSummary(hours, user);

            db.Users.Add(manager);
            db.Users.Add(user);
            db.SaveChanges();


            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheetObsolete("token", timeSheet);

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response);
                Assert.Equal(0, db.Payslips.Count());


            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #endregion

        #region LogHours
        [Fact]
        public async void TimeSheetController_LogHours_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00)
            });

            db.Users.Add(user);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = "NonexistentSession" }));
            try
            {
                //Act
                var response = await timeSheetController.LogHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHours_With_InvalidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2021, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00)
            });

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = token.Token }));
            try
            {
                //Act
                var response = await timeSheetController.LogHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHours_With_ExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
                Id = 0

            };

            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00).ToLocalTime()
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00).ToLocalTime()
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00).ToLocalTime()
            });

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = token.Token }));
            try
            {
                //Act
                var response = await timeSheetController.LogHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHours_With_ValidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 15, 00, 00)
            });

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = token.Token }));

            try
            {
                //Act
                var response = await timeSheetController.LogHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(3, db.WorkHours.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_LogHours_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
           
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "" }));

            try
            {
                //Act
                var response = await timeSheetController.LogHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        [Fact]
        public async void TimeSheetController_LogHours_With_IncorrectSubmissionSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = "awd" }));

            try
            {
                //Act
                var response = await timeSheetController.LogHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetHours
        [Fact]
        public async void TimeSheetController_GetHours_With_ExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { day = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(), sessionToken = token.Token }));
            try
            {
                //Act
                var response = await timeSheetController.GetHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<string>(response.Value);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetHours_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 15, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 17, 00, 00),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { day = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime(), sessionToken = "InexistentSession" }));

            try
            {
                //Act
                var response = await timeSheetController.GetHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetHours_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "" }));

            try
            {
                //Act
                var response = await timeSheetController.GetHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }


        [Fact]
        public async void TimeSheetController_GetHours_With_IncorrectDaySchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { dau = "fas" }));

            try
            {
                //Act
                var response = await timeSheetController.GetHours(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetTimeSheet
        [Fact]
        public async void TimeSheetController_GetTimeSheet_With_ValidSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 10, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 12, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { date = workHours.Date, sessionToken = token.Token }));

            try
            {
                //Act
                var response = await timeSheetController.GetTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<WorkHoursInfo>>>(un);
                Assert.Equal(7, responseValue.Count());
                Assert.Equal(2, responseValue.ToList()[0].Count());
                Assert.Single(responseValue.ToList()[2]);
                Assert.Single(responseValue.ToList()[6]);
                Assert.Equal(workHours3.StartTime, responseValue.ToList()[6].First().StartTime);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetTimeSheet_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { date = new DateTime(2022, 11, 26, 00, 00, 00), sessionToken = "NonexistentSession" }));
            try
            {
                //Act
                var response = await timeSheetController.GetTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetTimeSheet_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "Non" }));
            try
            {
                //Act
                var response = await timeSheetController.GetTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetTimeSheet_With_IncorrectDateSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { date = "ads", sessionToken = "NonexistentSession" }));
            try
            {
                //Act
                var response = await timeSheetController.GetTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region SubmitTimeSheet
        [Fact]
        public async void TimeSheetController_SubmitTimeSheet_With_ApprovedSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 20, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Approved.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 20, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 21, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 21, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 23, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 26, 00, 00, 00).ToLocalTime(),
                StartTime = new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 18, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = token.Token }));
            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_SubmitTimeSheet_With_ValidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SessionTokens.Add(token);
            db.SaveChanges();

            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 23, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 18, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            });

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = token.Token }));

            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(4, db.WorkHours.Count());
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 20, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 21, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 23, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)));
                Assert.Equal(db.WorkHours.First(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)).StartTime, new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime());

            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_SubmitTimeSheet_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission = submission, sessionToken = "NonexistentSession" }));
            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_SubmitTimeSheet_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField= "ion" }));
            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_SubmitTimeSheet_With_IncorrectSubmissionSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);


            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { submission =  "Nonexistent" }));
            try
            {
                //Act
                var response = await timeSheetController.SubmitTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region GetSubmittedTimeSheets
        [Fact]
        public async void TimeSheetController_GetSubmittedTimeSheets_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = manager,
            };

            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            User user1 = new User()
            {
                Id = "Tst_000002",
                Name = "John Smith",
                Email = "test2@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 17, 30, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 23, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Sick.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours4 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours5 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours6 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours7 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Holiday.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours8 = new WorkHours()
            {
                User = user1,
                Date = new DateTime(2022, 11, 28, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 28, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 28, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            db.Users.Add(manager);
            db.Users.Add(user);
            db.Users.Add(user1);
            db.SessionTokens.Add(token);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.WorkHours.Add(workHours4);
            db.WorkHours.Add(workHours5);
            db.WorkHours.Add(workHours6);
            db.WorkHours.Add(workHours7);
            db.WorkHours.Add(workHours8);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token }));

            try
            {
                //Act
                var response = await timeSheetController.GetSubmittedTimeSheets(new EncryptedData(data));

                //Assert
                Assert.NotNull(response.Value);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<List<TimeSheetSummary>>(un);

                Assert.Equal(3, responseValue.Count());
                Assert.Equal(28.5, responseValue.First(x => x.UserId == user.Id).HoursWorked);
                Assert.Equal(8, responseValue.First(x => x.UserId == user.Id).PaidSick);
                Assert.Equal(8, responseValue.First(x => x.UserId == user1.Id).PaidHoliday);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetSubmittedTimeSheets_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(manager);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = "NoToken" }));

            try
            {
                //Act
                var response = await timeSheetController.GetSubmittedTimeSheets(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_GetSubmittedTimeSheets_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(manager);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { session = "NoToken" }));

            try
            {
                //Act
                var response = await timeSheetController.GetSubmittedTimeSheets(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

        #region ApproveTimeSheet
        [Fact]
        public async void TimeSheetController_ApproveTimeSheet_With_NoSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };
            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = manager,
            };
            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };


            db.Users.Add(manager);
            db.Users.Add(user);
            db.Add(token);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token , timeSheet = new TimeSheetSummary() }));
            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Equal(0, db.Payslips.Count());

            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_ApproveTimeSheet_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = manager,
            };

            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 21, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 21, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 21, 17, 30, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 23, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 23, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 23, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Sick.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours4 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            WorkHours workHours5 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 28, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 28, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 28, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            List<WorkHours> hours = new List<WorkHours>();
            hours.Add(workHours);
            hours.Add(workHours1);
            hours.Add(workHours2);
            hours.Add(workHours3);
            hours.Add(workHours4);
            hours.Add(workHours5);

            TimeSheetSummary timeSheet = new TimeSheetSummary(hours, user);

            db.Users.Add(manager);
            db.Users.Add(user);
            db.Add(token);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.WorkHours.Add(workHours4);
            db.WorkHours.Add(workHours5);
            db.SaveChanges();

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = token.Token, timeSheet = timeSheet }));
            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<OkResult>(response);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours1.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours2.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours3.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours4.Status);
                Assert.Equal(WorkHourStatus.Submitted.ToString(), workHours5.Status);
                Assert.Equal(1, db.Payslips.Count());


            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_ApproveTimeSheet_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User manager = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Password = EncryptionHelper.EncryptString("password")
            };

            User user = new User()
            {
                Id = "Tst_000001",
                Name = "John Smith",
                Email = "test1@gmail.com",
                Password = EncryptionHelper.EncryptString("password"),
                Manager = manager
            };

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 17, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Submitted.ToString(),
                Id = 0

            };

            List<WorkHours> hours = new List<WorkHours>();
            hours.Add(workHours);

            TimeSheetSummary timeSheet = new TimeSheetSummary(hours, user);

            db.Users.Add(manager);
            db.Users.Add(user);
            db.SaveChanges();


            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = "NoToken", timeSheet = timeSheet }));
            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundObjectResult>(response);
                Assert.Equal(0, db.Payslips.Count());


            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_ApproveTimeSheet_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "asd"}));
            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Equal(0, db.Payslips.Count());


            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void TimeSheetController_ApproveTimeSheet_With_IncorrectTimeSheetSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            ISessionService sessionService = new SessionService(db);
            IPayslipService payslipService = new PayslipService(db);
            TimeSheetController timeSheetController = new TimeSheetController(timeSheetService, sessionService, payslipService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = "NoToken", timeSheet = "fdsa" }));
            try
            {
                //Act
                var response = await timeSheetController.ApproveTimeSheet(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.Equal(0, db.Payslips.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
        #endregion

    }
}
