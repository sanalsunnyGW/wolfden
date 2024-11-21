using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfDen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LeaveFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncrementGap",
                schema: "wolfden",
                table: "LeaveType")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "wolfden",
                table: "LeaveType",
                newName: "IncrementGapId")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "LeaveRequestStatus",
                schema: "wolfden",
                table: "LeaveRequest",
                newName: "LeaveRequestStatusId")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveRequest")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<int>(
                name: "LeaveCategoryId",
                schema: "wolfden",
                table: "LeaveType",
                type: "int",
                nullable: true,
                defaultValue: 7)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveCategoryId",
                schema: "wolfden",
                table: "LeaveType")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "IncrementGapId",
                schema: "wolfden",
                table: "LeaveType",
                newName: "Type")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "LeaveRequestStatusId",
                schema: "wolfden",
                table: "LeaveRequest",
                newName: "LeaveRequestStatus")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveRequest")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<int>(
                name: "IncrementGap",
                schema: "wolfden",
                table: "LeaveType",
                type: "int",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LeaveType")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
