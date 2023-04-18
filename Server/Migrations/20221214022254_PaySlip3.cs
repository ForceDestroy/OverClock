using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Server.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class PaySlip3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoubleOvertimeWorked",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "OvertimeWorked",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PaidHoliday",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PaidSick",
                table: "Payslips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DoubleOvertimeWorked",
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

            migrationBuilder.AddColumn<decimal>(
                name: "PaidHoliday",
                table: "Payslips",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidSick",
                table: "Payslips",
                type: "decimal (18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
