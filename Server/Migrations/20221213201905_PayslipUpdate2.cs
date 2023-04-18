using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Server.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class PayslipUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<decimal>(
                name: "AmountAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EIAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FITAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoursAccumulated",
                table: "Payslips",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QCPITAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QPIPAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QPPAccumulated",
                table: "Payslips",
                type: "Money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "EIAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "FITAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "HoursAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "QCPITAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "QPIPAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "QPPAccumulated",
                table: "Payslips");

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
        }
    }
}
