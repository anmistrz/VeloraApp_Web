using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addUniqueIdSalesPersonPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UQ_SalesPersonPerformance_Metric",
                table: "SalesPersonPerformance",
                columns: new[] { "SalesPersonID", "MetricType", "MetricDate" },
                unique: true,
                filter: "[SalesPersonID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_SalesPersonPerformance_Metric",
                table: "SalesPersonPerformance");
        }
    }
}
