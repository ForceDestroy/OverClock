using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Server.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class PaySlipUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "FederalTax",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "ProvincialTax",
                table: "Payslips");

            migrationBuilder.RenameColumn(
                name: "HoursCurrent",
                table: "Payslips",
                newName: "PaidSick");

            migrationBuilder.RenameColumn(
                name: "HoursAccumulated",
                table: "Payslips",
                newName: "PaidHoliday");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountAccumulated",
                table: "Users",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EIAccumulated",
                table: "Users",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FITAccumulated",
                table: "Users",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoursAccumulated",
                table: "Users",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QCPITAccumulated",
                table: "Users",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QPIPAccumulated",
                table: "Users",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QPPAccumulated",
                table: "Users",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DoubleOvertimeWorked",
                table: "Payslips",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoursWorked",
                table: "Payslips",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OvertimeWorked",
                table: "Payslips",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EIAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FITAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HoursAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QCPITAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QPIPAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QPPAccumulated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DoubleOvertimeWorked",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "HoursWorked",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "OvertimeWorked",
                table: "Payslips");

            migrationBuilder.RenameColumn(
                name: "PaidSick",
                table: "Payslips",
                newName: "HoursCurrent");

            migrationBuilder.RenameColumn(
                name: "PaidHoliday",
                table: "Payslips",
                newName: "HoursAccumulated");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FederalTax",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProvincialTax",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
