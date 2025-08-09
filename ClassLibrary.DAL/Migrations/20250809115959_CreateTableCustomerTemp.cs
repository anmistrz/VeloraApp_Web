using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableCustomerTemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTemps",
                columns: table => new
                {
                    CustomerTempID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    District = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Province = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTemps", x => x.CustomerTempID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTemps");
        }
    }
}
