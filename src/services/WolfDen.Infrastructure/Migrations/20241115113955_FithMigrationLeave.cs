using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfDen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FithMigrationLeave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hidden",
                schema: "wolfden",
                table: "LeaveType")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                schema: "wolfden",
                table: "LeaveType",
                type: "bit",
                nullable: true,
                defaultValue: false)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
