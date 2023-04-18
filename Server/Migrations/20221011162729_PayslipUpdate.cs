using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Server.Migrations
{
    public partial class PayslipUpdate : Migration
    {
        [ExcludeFromCodeCoverage]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "GrossAmount",
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
                name: "HoursCurrent",
                table: "Payslips",
                type: "decimal (18,2)",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "FederalTax",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "HoursAccumulated",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "HoursCurrent",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "ProvincialTax",
                table: "Payslips");
        }
    }
}
