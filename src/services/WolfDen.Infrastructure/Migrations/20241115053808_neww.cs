using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfDen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class neww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendenceLog_DailyAttendence_DailyAttendenceId",
                schema: "wolfden",
                table: "AttendenceLog");

            migrationBuilder.DropIndex(
                name: "IX_AttendenceLog_DailyAttendenceId",
                schema: "wolfden",
                table: "AttendenceLog");

            migrationBuilder.DropColumn(
                name: "DailyAttendenceId",
                schema: "wolfden",
                table: "AttendenceLog")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AttendenceLog")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyAttendenceId",
                schema: "wolfden",
                table: "AttendenceLog",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AttendenceLog")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_AttendenceLog_DailyAttendenceId",
                schema: "wolfden",
                table: "AttendenceLog",
                column: "DailyAttendenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendenceLog_DailyAttendence_DailyAttendenceId",
                schema: "wolfden",
                table: "AttendenceLog",
                column: "DailyAttendenceId",
                principalSchema: "wolfden",
                principalTable: "DailyAttendence",
                principalColumn: "DailyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
