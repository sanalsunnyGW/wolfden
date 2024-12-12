using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfDen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class defaultLeaveSettingDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "wolfden",
                table: "LeaveSetting",
                columns: new[] { "LeaveSettingId", "MaxNegativeBalanceLimit", "MinDaysForLeaveCreditJoining" },
                values: new object[] { 1, 2, 15 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "wolfden",
                table: "LeaveSetting",
                keyColumn: "LeaveSettingId",
                keyValue: 1);
        }
    }
}
