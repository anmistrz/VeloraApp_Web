using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class columnStatusConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatusConsultation",
                table: "ConsultHistory",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ConsultHistory",
                keyColumn: "ConsultHistoryID",
                keyValue: 1,
                column: "StatusConsultation",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "ConsultHistory",
                keyColumn: "ConsultHistoryID",
                keyValue: 2,
                column: "StatusConsultation",
                value: "Pending");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatusConsultation",
                table: "ConsultHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Pending");

            migrationBuilder.UpdateData(
                table: "ConsultHistory",
                keyColumn: "ConsultHistoryID",
                keyValue: 1,
                column: "StatusConsultation",
                value: null);

            migrationBuilder.UpdateData(
                table: "ConsultHistory",
                keyColumn: "ConsultHistoryID",
                keyValue: 2,
                column: "StatusConsultation",
                value: null);
        }
    }
}
