using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfDen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class datass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyAttendence_Status_StatusId",
                schema: "wolfden",
                table: "DailyAttendence");

            migrationBuilder.DropIndex(
                name: "IX_DailyAttendence_StatusId",
                schema: "wolfden",
                table: "DailyAttendence");

            migrationBuilder.DropColumn(
                name: "StatusId",
                schema: "wolfden",
                table: "DailyAttendence")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DailyAttendence")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                schema: "wolfden",
                table: "DailyAttendence",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DailyAttendence")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "wolfdenHT")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendence_StatusId",
                schema: "wolfden",
                table: "DailyAttendence",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAttendence_Status_StatusId",
                schema: "wolfden",
                table: "DailyAttendence",
                column: "StatusId",
                principalSchema: "wolfden",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
