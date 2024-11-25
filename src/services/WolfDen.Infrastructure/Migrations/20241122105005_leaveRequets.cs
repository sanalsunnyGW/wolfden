using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfDen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class leaveRequets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LOP_EmployeeId",
                schema: "wolfden",
                table: "LOP",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LOP_Employee_EmployeeId",
                schema: "wolfden",
                table: "LOP",
                column: "EmployeeId",
                principalSchema: "wolfden",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOP_Employee_EmployeeId",
                schema: "wolfden",
                table: "LOP");

            migrationBuilder.DropIndex(
                name: "IX_LOP_EmployeeId",
                schema: "wolfden",
                table: "LOP");
        }
    }
}
