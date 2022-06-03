using Microsoft.EntityFrameworkCore.Migrations;

namespace UberAPI.Migrations
{
    public partial class changingFkInDriversLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DriversLocations_DriversId",
                table: "DriversLocations",
                column: "DriversId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriversLocations_Users_DriversId",
                table: "DriversLocations",
                column: "DriversId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriversLocations_Users_DriversId",
                table: "DriversLocations");

            migrationBuilder.DropIndex(
                name: "IX_DriversLocations_DriversId",
                table: "DriversLocations");
        }
    }
}
