﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230322175312_ModuleStatusUpdate")]
    partial class ModuleStatusUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Server.DbModels.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Server.DbModels.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Experience")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostingId")
                        .HasColumnType("int");

                    b.Property<string>("Referral")
                        .IsRequired()
                        .HasColumnType("char");

                    b.Property<string>("SkillSet")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("PostingId");

                    b.HasIndex("UserId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Server.DbModels.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FromId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ToId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("Server.DbModels.JobPosting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Salary")
                        .HasColumnType("Money");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("JobPostings");
                });

            modelBuilder.Entity("Server.DbModels.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("Server.DbModels.ModuleStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.HasIndex("UserId");

                    b.ToTable("ModuleStatus");
                });

            modelBuilder.Entity("Server.DbModels.PaySlip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("AmountAccumulated")
                        .HasColumnType("Money");

                    b.Property<decimal>("EIAccumulated")
                        .HasColumnType("Money");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("Date");

                    b.Property<decimal>("FITAccumulated")
                        .HasColumnType("Money");

                    b.Property<decimal>("GrossAmount")
                        .HasColumnType("Money");

                    b.Property<decimal>("HoursAccumulated")
                        .HasColumnType("decimal (18,2)");

                    b.Property<decimal>("HoursWorked")
                        .HasColumnType("decimal (18,2)");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("Date");

                    b.Property<decimal>("NetAccumulated")
                        .HasColumnType("Money");

                    b.Property<decimal>("QCPITAccumulated")
                        .HasColumnType("Money");

                    b.Property<decimal>("QPIPAccumulated")
                        .HasColumnType("Money");

                    b.Property<decimal>("QPPAccumulated")
                        .HasColumnType("Money");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("Date");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Payslips");
                });

            modelBuilder.Entity("Server.DbModels.Policy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<string>("Hyperlink")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Policies");
                });

            modelBuilder.Entity("Server.DbModels.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("Date");

                    b.Property<string>("FromId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("Date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ToId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Server.DbModels.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("AllowedBreakTime")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Server.DbModels.SessionToken", b =>
                {
                    b.Property<string>("Token")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Token");

                    b.HasIndex("UserId");

                    b.ToTable("SessionTokens");
                });

            modelBuilder.Entity("Server.DbModels.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("BankingNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("Date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ManagerID")
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SIN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("Money");

                    b.Property<int>("SickDays")
                        .HasColumnType("int");

                    b.Property<int>("ThemeColor")
                        .HasColumnType("int");

                    b.Property<int>("VacationDays")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ManagerID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Server.DbModels.WorkHours", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Function")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("WorkHours");
                });

            modelBuilder.Entity("Server.DbModels.Announcement", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("Announcements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.Application", b =>
                {
                    b.HasOne("Server.DbModels.JobPosting", "Posting")
                        .WithMany("Applications")
                        .HasForeignKey("PostingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("Applications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Posting");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.Email", b =>
                {
                    b.HasOne("Server.DbModels.User", "From")
                        .WithMany("SentEmails")
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Server.DbModels.User", "To")
                        .WithMany("ReceivedEmails")
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("From");

                    b.Navigation("To");
                });

            modelBuilder.Entity("Server.DbModels.JobPosting", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("JobPostings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.Module", b =>
                {
                    b.HasOne("Server.DbModels.User", "Employer")
                        .WithMany("Modules")
                        .HasForeignKey("UserId");

                    b.Navigation("Employer");
                });

            modelBuilder.Entity("Server.DbModels.ModuleStatus", b =>
                {
                    b.HasOne("Server.DbModels.Module", "Module")
                        .WithMany("Statuses")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.DbModels.User", "Employee")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Employee");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("Server.DbModels.PaySlip", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("Payslips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.Policy", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("Policies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.Request", b =>
                {
                    b.HasOne("Server.DbModels.User", "From")
                        .WithMany("SentRequests")
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Server.DbModels.User", "To")
                        .WithMany("ReceivedRequests")
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("From");

                    b.Navigation("To");
                });

            modelBuilder.Entity("Server.DbModels.Schedule", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("Schedules")
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.SessionToken", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.User", b =>
                {
                    b.HasOne("Server.DbModels.User", "Manager")
                        .WithMany("Manages")
                        .HasForeignKey("ManagerID");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Server.DbModels.WorkHours", b =>
                {
                    b.HasOne("Server.DbModels.User", "User")
                        .WithMany("WorkHours")
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.DbModels.JobPosting", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("Server.DbModels.Module", b =>
                {
                    b.Navigation("Statuses");
                });

            modelBuilder.Entity("Server.DbModels.User", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Applications");

                    b.Navigation("JobPostings");

                    b.Navigation("Manages");

                    b.Navigation("Modules");

                    b.Navigation("Payslips");

                    b.Navigation("Policies");

                    b.Navigation("ReceivedEmails");

                    b.Navigation("ReceivedRequests");

                    b.Navigation("Schedules");

                    b.Navigation("SentEmails");

                    b.Navigation("SentRequests");

                    b.Navigation("WorkHours");
                });
#pragma warning restore 612, 618
        }
    }
}
