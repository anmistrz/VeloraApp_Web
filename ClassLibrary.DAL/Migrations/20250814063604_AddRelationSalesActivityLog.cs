using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationSalesActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTemps");

            migrationBuilder.AddColumn<int>(
                name: "ConsultationID",
                table: "SalesActivityLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestDriveID",
                table: "SalesActivityLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesActivityLog_ConsultationID",
                table: "SalesActivityLog",
                column: "ConsultationID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesActivityLog_TestDriveID",
                table: "SalesActivityLog",
                column: "TestDriveID");

            migrationBuilder.AddForeignKey(
                name: "FK__SalesActi__Consu__74A0C674",
                table: "SalesActivityLog",
                column: "ConsultationID",
                principalTable: "ConsultHistory",
                principalColumn: "ConsultHistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK__SalesActi__TestD__75A278F5",
                table: "SalesActivityLog",
                column: "TestDriveID",
                principalTable: "TestDrive",
                principalColumn: "TestDriveID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__SalesActi__Consu__74A0C674",
                table: "SalesActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK__SalesActi__TestD__75A278F5",
                table: "SalesActivityLog");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_SalesActivityLog_ConsultationID",
                table: "SalesActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_SalesActivityLog_TestDriveID",
                table: "SalesActivityLog");

            migrationBuilder.DropColumn(
                name: "ConsultationID",
                table: "SalesActivityLog");

            migrationBuilder.DropColumn(
                name: "TestDriveID",
                table: "SalesActivityLog");

            migrationBuilder.CreateTable(
                name: "CustomerTemps",
                columns: table => new
                {
                    CustomerTempID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    District = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Province = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTemps", x => x.CustomerTempID);
                });
        }
    }
}
