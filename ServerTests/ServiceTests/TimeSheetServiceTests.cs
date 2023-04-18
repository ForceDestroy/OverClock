using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.BOModels;

namespace ServerTests.ServiceTests
{
    [Collection("Sequential")]
    public class TimeSheetServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        public TimeSheetServiceTests()
        {
        }
        #region CheckDaysSubmission
        [Fact]
        public async void TimeSheetService_CheckDaysSubmission_With_ExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetService.CheckDaysSubmission(user.Id, new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime());

                //Assert
                Assert.True(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void TimeSheetService_CheckDaysSubmission_With_NoExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            db.Users.Add(user);
            db.SaveChanges();

            try
            {
                //Act
                var response = await timeSheetService.CheckDaysSubmission(user.Id, new DateTime(2020, 01, 01, 00, 00, 00));

                //Assert
                Assert.False(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        # endregion ValidateWorkHours

        #region ValidateWorkHours
        [Fact]
        public void TimeSheetService_ValidateWorkHours_With_ValidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo() {
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
                var response = timeSheetService.ValidateWorkHours(submission);

                //Assert
                Assert.True(response);
            }
            finally
            {
                //Dispose
            }
        }

        [Fact]
        public void TimeSheetService_ValidateWorkHours_With_InvalidSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            List<WorkHoursInfo> submission = new List<WorkHoursInfo>();
            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 01, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2020, 01, 01, 10, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 02, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 02, 01, 11, 00, 00),
                EndTime = new DateTime(2020, 02, 01, 12, 00, 00)
            });

            submission.Add(new WorkHoursInfo()
            {
                Date = new DateTime(2020, 03, 01, 00, 00, 00),
                StartTime = new DateTime(2020, 03, 01, 14, 00, 00),
                EndTime = new DateTime(2020, 03, 01, 15, 00, 00)
            });

            try
            {
                //Act
                var response = timeSheetService.ValidateWorkHours(submission);

                //Assert
                Assert.False(response);
            }
            finally
            {
                //Dispose
            }
        }
        #endregion

        #region LogWorkHours
        [Fact]
        public async void TimeSheetService_LogWorkHours_With_NoUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
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
                var response = await timeSheetService.LogWorkHours("SomeUserID", submission);

                //Assert
                Assert.False(response);
                Assert.Equal(0, db.WorkHours.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void TimeSheetService_LogWorkHours_With_ValidUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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

            db.Users.Add(user);
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
                var response = await timeSheetService.LogWorkHours(user.Id, submission);

                //Assert
                Assert.True(response);
                Assert.Equal(3, db.WorkHours.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region GetTimeSheet
        [Fact]
        public async void TimeSheetService_GetTimeSheet_With_NoUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange

            try
            {
                //Act
                var response = await timeSheetService.GetTimeSheet("USERID", DateTime.Now);

                //Assert
                Assert.Empty(response);
            }
            finally
            {
                //Dispose
            }
        }

        [Fact]
        public async void TimeSheetService_GetTimeSheet_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 05, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
                Id = 0

            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 15, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 16, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
                Id = 0

            };

            WorkHours workHours2 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 22, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 22, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 22, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
                Id = 0

            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
                Id = 0

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
            db.SaveChanges();


            try
            {
                //Act
                var response = (await timeSheetService.GetTimeSheet(user.Id, new DateTime(2022, 11, 26, 05, 00, 00).ToLocalTime())).ToList();
                

                //Assert
                Assert.NotNull(response);
                Assert.Equal(7, response.Count());
                Assert.Equal(2, response[0].Count());
                Assert.Single(response[2]);
                Assert.Single(response[6]);
                Assert.Equal(workHours3.StartTime, response[6].First().StartTime);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region SubmitTimeSheet
        

        [Fact]
        public async void TimeSheetService_SubmitTimeSheet_With_EqualSubmissionAndDB()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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
                await timeSheetService.SubmitTimeSheet(user.Id, submission);

                //Assert
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
            }
        }
        [Fact]
        public async void TimeSheetService_SubmitTimeSheet_With_NoUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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
                await timeSheetService.SubmitTimeSheet("wrongID", submission);

                //Assert
                Assert.Equal(4, db.WorkHours.Count());
                Assert.Equal(db.WorkHours.First(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)).StartTime, new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime());

            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        [Fact]
        public async void TimeSheetService_SubmitTimeSheet_With_HolidayOrSick()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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
                Function = WorkHourFunctions.Sick.ToString(),
                Status = WorkHourStatus.Draft.ToString(),
            };

            WorkHours workHours3 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2022, 11, 26, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 26, 09, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 26, 13, 00, 00).ToLocalTime(),
                Function = WorkHourFunctions.Holiday.ToString(),
                Status = WorkHourStatus.Draft.ToString(),

            };


            db.Users.Add(user);
            db.WorkHours.Add(workHours);
            db.WorkHours.Add(workHours1);
            db.WorkHours.Add(workHours2);
            db.WorkHours.Add(workHours3);
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


            try
            {
                //Act
                await timeSheetService.SubmitTimeSheet(user.Id, submission);

                //Assert
                Assert.Equal(4, db.WorkHours.Count());
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 20, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 21, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 23, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)));
                Assert.Equal(workHours2.Function, WorkHourFunctions.Sick.ToString());
                Assert.Equal(workHours3.Function, WorkHourFunctions.Holiday.ToString());
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        [Fact]
        public async void TimeSheetService_SubmitTimeSheet_With_MoreSubmissionThanDB()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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
                Date = new DateTime(2022, 11, 20, 00, 00, 00),
                StartTime = new DateTime(2022, 11, 20, 20, 00, 00).ToLocalTime(),
                EndTime = new DateTime(2022, 11, 20, 23, 00, 00).ToLocalTime(),
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
                await timeSheetService.SubmitTimeSheet(user.Id, submission);

                //Assert
                Assert.Equal(5, db.WorkHours.Count());
                Assert.Equal(2,db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 20, 00, 00, 00)).Count());
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
            }
        }

        [Fact]
        public async void TimeSheetService_SubmitTimeSheet_With_MoreDbThanSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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
                await timeSheetService.SubmitTimeSheet(user.Id, submission);

                //Assert
                Assert.Equal(3, db.WorkHours.Count());
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 20, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 23, 00, 00, 00)));
                Assert.NotNull(db.WorkHours.Where(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)));
                Assert.Equal(db.WorkHours.First(x => x.Date == new DateTime(2022, 11, 26, 00, 00, 00)).StartTime, new DateTime(2022, 11, 26, 07, 00, 00).ToLocalTime());
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region GetWorkHours
          [Fact]
        public async void TimeSheetService_GetWorkHours_With_ExistingSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
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

            WorkHours workHours = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime().ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 09, 00, 00).ToLocalTime().ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 13, 00, 00).ToLocalTime().ToLocalTime(),
                Function = WorkHourFunctions.Work.ToString(),
                Status = WorkHourStatus.Draft.ToString()
            };

            WorkHours workHours1 = new WorkHours()
            {
                User = user,
                Date = new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime().ToLocalTime(),
                StartTime = new DateTime(2020, 01, 01, 15, 00, 00).ToLocalTime().ToLocalTime(),
                EndTime = new DateTime(2020, 01, 01, 17, 00, 00).ToLocalTime().ToLocalTime(),
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
                var response = await timeSheetService.GetWorkHours(user.Id, new DateTime(2020, 01, 01, 00, 00, 00).ToLocalTime());

                //Assert
                Assert.NotNull(response);
                Assert.Equal(2, response.Count());
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void TimeSheetService_GetWorkHours_With_NoUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);
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

            try
            {
                //Act
                var response = await timeSheetService.GetWorkHours(user.Id, new DateTime(2020, 01, 01, 00, 00, 00));

                //Assert
                Assert.Empty(response);
            }
            finally
            {
                //Dispose
                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region GetSubmittedTimeSheets
        [Fact]
        public async void TimeSheetService_GetSubmittedTimeSheets_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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
                var response = await timeSheetService.GetSubmittedTimeSheets(manager.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Equal(3, response.Count());
                Assert.Equal(28.5, response.First(x => x.UserId == user.Id).HoursWorked);
                Assert.Equal(8, response.First(x => x.UserId == user.Id).PaidSick);
                Assert.Equal(8, response.First(x => x.UserId == user1.Id).PaidHoliday);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region ApproveTimeSheet
        [Fact]
        public async void TimeSheetService_ApproveTimeSheet_With_NoSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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


            db.Users.Add(manager);
            db.Users.Add(user);
            db.SaveChanges();


            try
            {
                //Act
                var response = await timeSheetService.ApproveTimeSheet(user.Id , new DateTime(2022, 11, 20, 00, 00, 00));

                //Assert
                Assert.False(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void TimeSheetService_ApproveTimeSheet_With_ValidInput()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

            db.Database.ExecuteSqlRaw("delete from WorkHours");
            db.Database.ExecuteSqlRaw("delete from Users");

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


            db.Users.Add(manager);
            db.Users.Add(user);
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
                var response = await timeSheetService.ApproveTimeSheet(user.Id,new DateTime(2022, 11, 20, 00, 00, 00));

                //Assert
                Assert.True(response);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours1.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours2.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours3.Status);
                Assert.Equal(WorkHourStatus.Approved.ToString(), workHours4.Status);
                Assert.Equal(WorkHourStatus.Submitted.ToString(), workHours5.Status);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        #endregion

        #region CheckForApproveTimeSheet
        [Fact]
        public async void TimeSheetService_CheckForApprovedSubmission_With_ApprovedSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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
                var response = await timeSheetService.CheckForApprovedTimeSheet(user.Id, submission.First().Date);

                //Assert
                Assert.True(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        public async void TimeSheetService_CheckForApprovedSubmission_With_NoApprovedSubmission()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            ITimeSheetService timeSheetService = new TimeSheetService(db);

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

            db.Users.Add(user);
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
                var response = await timeSheetService.CheckForApprovedTimeSheet(user.Id, submission.First().Date);

                //Assert
                Assert.False(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from WorkHours");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion
    }
}
