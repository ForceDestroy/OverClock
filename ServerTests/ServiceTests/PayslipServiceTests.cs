using Server.Data;
using Server.DbModels;
using Server.Enums;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
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
    public class PayslipServiceTests : TestBase
    {
        private string _connectionString = System.IO.File.ReadAllText("TestDbConnectionString.txt");
        public PayslipServiceTests()
        {
        }
        #region CreatePayslip

        [Fact]
        public async void PayslipService_CreatePayslip_With_NoExistingPayslip()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(user);
            db.SaveChanges();

            TimeSheetSummary timeSheet = new TimeSheetSummary()
            {
                UserId = user.Id,
                Name = user.Name,
                Salary = user.Salary,
                HoursWorked = 40,
                OvertimeWorked = 20,
                DoubleOvertimeWorked = 10,
                PaidSick = 8,
                PaidHoliday = 4,
            };

            try
            {
                //Act
                var response = await payslipService.CreatePayslip(timeSheet);

                //Assert
                Assert.True(response);
                Assert.Equal(1, db.Payslips.Count());
                Assert.Equal(70, db.Payslips.First().HoursWorked);
                Assert.Equal(70, db.Payslips.First().HoursAccumulated);
                Assert.Equal(1020, db.Payslips.First().GrossAmount);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void PayslipService_CreatePayslip_With_NoUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
            };

            TimeSheetSummary timeSheet = new TimeSheetSummary()
            {
                UserId = user.Id,
                Name = user.Name,
                Salary = user.Salary,
                HoursWorked = 40,
                OvertimeWorked = 20,
                DoubleOvertimeWorked = 10,
                PaidSick = 8,
                PaidHoliday = 4,
            };

            try
            {
                //Act
                var response = await payslipService.CreatePayslip(timeSheet);

                //Assert
                Assert.False(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void PayslipService_CreatePayslip_With_ExistingPayslip()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
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
            };

            db.Users.Add(user);
            db.Payslips.Add(payslip);
            db.SaveChanges();

            TimeSheetSummary timeSheet = new TimeSheetSummary()
            {
                UserId = user.Id,
                Name = user.Name,
                Salary = user.Salary,
                HoursWorked = 00,
            };

            try
            {
                //Act
                var response = await payslipService.CreatePayslip(timeSheet);

                //Assert
                Assert.True(response);
                Assert.Equal(2, db.Payslips.Count());
                Assert.Equal(10, db.Payslips.First().HoursAccumulated);
                Assert.Equal(10, db.Payslips.First().AmountAccumulated);
                Assert.Equal(10, db.Payslips.First().EIAccumulated);
                Assert.Equal(10, db.Payslips.First().FITAccumulated);
                Assert.Equal(10, db.Payslips.First().QPPAccumulated);
                Assert.Equal(10, db.Payslips.First().QPIPAccumulated);
                Assert.Equal(10, db.Payslips.First().QCPITAccumulated);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion

        #region GetPayslips
        [Fact]
        public async void PayslipService_GetPayslips_With_NoUser()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
            };


            try
            {
                //Act
                var response = await payslipService.GetPayslips(user.Id);

                //Assert
                Assert.Empty(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void PayslipService_GetPayslips_With_ExistingPayslip()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
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
                StartDate= DateTime.Now.AddDays(7),
            };

            db.Users.Add(user);
            db.Payslips.Add(payslip);
            db.Payslips.Add(payslip2);
            db.SaveChanges();

            try
            {
                //Act
                var response = await payslipService.GetPayslips(user.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Equal(2, response.Count());
                Assert.True(string.Equals(user.Name, response.First().Name));
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }

        [Fact]
        public async void PayslipService_GetPayslips_With_NoPayslip()
        {
            //Initialize
            DatabaseContext db = new DatabaseContext(new Dbconnection(_connectionString));
            IPayslipService payslipService = new PayslipService(db);

            db.Database.ExecuteSqlRaw("delete from PaySlips");
            db.Database.ExecuteSqlRaw("delete from Users");

            //Arrange
            User user = new User()
            {
                Id = "Tst_000000",
                Name = "John Smith",
                Email = "test@gmail.com",
                Salary = 10,
                Password = EncryptionHelper.EncryptString("password")
            };

            db.Users.Add(user);
            db.SaveChanges();

            try
            {
                //Act
                var response = await payslipService.GetPayslips(user.Id);

                //Assert
                Assert.NotNull(response);
                Assert.Empty(response);
            }
            finally
            {
                //Dispose

                db.Database.ExecuteSqlRaw("delete from PaySlips");
                db.Database.ExecuteSqlRaw("delete from Users");
            }
        }
        #endregion
    }
}
