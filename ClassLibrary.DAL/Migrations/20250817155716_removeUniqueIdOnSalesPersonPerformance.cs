using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeUniqueIdOnSalesPersonPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_SalesPersonPerformance_Metric",
                table: "SalesPersonPerformance");

            migrationBuilder.AlterColumn<int>(
                name: "SalesPersonID",
                table: "SalesPersonPerformance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SalesPersonID",
                table: "SalesPersonPerformance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "UQ_SalesPersonPerformance_Metric",
                table: "SalesPersonPerformance",
                columns: new[] { "SalesPersonID", "MetricType", "MetricDate" },
                unique: true);
        }
    }
}
