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
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Server.BOModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ServerTests.ControllerTests
{
    [Collection("Sequential")]

    public class PaySlipControllerTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");

        public PaySlipControllerTests()
        {

        }
        #region Obsolete
        [Fact]
        public async void PayslipController_GetPayslipsObsolete_With_ExistingPayslip()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);
            ISessionService sessionService = new SessionService(db);
            PaySlipController paySlipController = new PaySlipController(payslipService, sessionService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            PaySlip payslip = new PaySlip()
            {
                User = user,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
                StartDate = DateTime.Now,
            };

            PaySlip payslip2 = new PaySlip()
            {
                User = user,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
                StartDate = DateTime.Now.AddDays(7),
            };

            PaySlip payslip3 = new PaySlip()
            {
                User = user,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
                StartDate = DateTime.Now.AddDays(-7),
            };

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.Payslips.Add(payslip);
            db.Payslips.Add(payslip2);
            db.Payslips.Add(payslip3);
            db.SaveChanges();

            try
            {
                //Act
                var response = await paySlipController.GetPaySlipsObsolete(token.Token);

                //Assert
                Assert.NotNull(response.Value);
                Assert.Equal(3, response.Value.Count());
                Assert.True(string.Equals(user.Name, response.Value.First().Name));
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void PayslipController_GetPayslipsObsolete_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);
            ISessionService sessionService = new SessionService(db);
            PaySlipController paySlipController = new PaySlipController(payslipService, sessionService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
           
            try
            {
                //Act
                var response = await paySlipController.GetPaySlipsObsolete("NOSESSION");

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        #endregion

        [Fact]
        public async void PayslipController_GetPayslips_With_ExistingPayslip()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);
            ISessionService sessionService = new SessionService(db);
            PaySlipController paySlipController = new PaySlipController(payslipService, sessionService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
            };

            SessionToken token = new SessionToken()
            {
                Token = "TESTTOKEN",
                User = user,
            };

            PaySlip payslip = new PaySlip()
            {
                User = user,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
                StartDate = DateTime.Now,
            };

            PaySlip payslip2 = new PaySlip()
            {
                User = user,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
                StartDate = DateTime.Now.AddDays(7),
            };

            PaySlip payslip3 = new PaySlip()
            {
                User = user,
                HoursAccumulated = 10,
                AmountAccumulated = 10,
                EIAccumulated = 10,
                FITAccumulated = 10,
                QPPAccumulated = 10,
                QPIPAccumulated = 10,
                QCPITAccumulated = 10,
                StartDate = DateTime.Now.AddDays(-7),
            };

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new {sessionToken = token.Token }));

            db.Users.Add(user);
            db.SessionTokens.Add(token);
            db.Payslips.Add(payslip);
            db.Payslips.Add(payslip2);
            db.Payslips.Add(payslip3);
            db.SaveChanges();

            try
            {
                //Act
                var response = await paySlipController.GetPaySlips(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                var un = EncryptionHelper.DecryptTLS(response.Value);
                var responseValue = JsonConvert.DeserializeObject<IEnumerable<PaySlipInfo>>(un);
                Assert.NotNull(responseValue);
                Assert.Equal(3, responseValue.Count());
                Assert.True(string.Equals(user.Name, responseValue.First().Name));
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void PayslipController_GetPayslips_With_NoSession()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);
            ISessionService sessionService = new SessionService(db);
            PaySlipController paySlipController = new PaySlipController(payslipService, sessionService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { sessionToken = "NOSESSION" }));

            try
            {
                //Act
                var response = await paySlipController.GetPaySlips(new EncryptedData(data));

                //Assert
                Assert.NotNull(response);
                Assert.IsType<NotFoundResult>(response.Result);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }

        [Fact]
        public async void PayslipController_GetPayslips_With_IncorrectSchema()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);
            ISessionService sessionService = new SessionService(db);
            PaySlipController paySlipController = new PaySlipController(payslipService, sessionService);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");
            db.Database.ExecuteSqlRaw("delete from SessionTokens");

            //Arrange
            

            var data = EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(new { randomField = "RandomValue" }));

            

            try
            {
                //Act
                var response = await paySlipController.GetPaySlips(new EncryptedData(data));

                //Assert
                Assert.Null(response.Value);
                Assert.IsType<BadRequestObjectResult>(response.Result);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
                db.Database.ExecuteSqlRaw("delete from SessionTokens");
            }
        }
    }
}
