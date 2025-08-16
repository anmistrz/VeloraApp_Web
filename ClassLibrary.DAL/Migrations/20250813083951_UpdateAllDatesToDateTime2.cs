using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllDatesToDateTime2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusConsultation",
                table: "ConsultHistory",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusConsultation",
                table: "ConsultHistory");
        }
    }
}
