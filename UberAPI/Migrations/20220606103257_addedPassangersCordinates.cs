using Microsoft.EntityFrameworkCore.Migrations;

namespace UberAPI.Migrations
{
    public partial class addedPassangersCordinates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PassangersCurrentLocationLatitude",
                table: "Reservations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PassangersCurrentLocationLongitude",
                table: "Reservations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PassangersDesiredLocationLatitude",
                table: "Reservations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PassangersDesiredLocationLongitude",
                table: "Reservations",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassangersCurrentLocationLatitude",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PassangersCurrentLocationLongitude",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PassangersDesiredLocationLatitude",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PassangersDesiredLocationLongitude",
                table: "Reservations");
        }
    }
}
