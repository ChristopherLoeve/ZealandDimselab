using Microsoft.EntityFrameworkCore.Migrations;

namespace ZealandDimselab.Migrations
{
    public partial class h : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_BookingId",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "NotificationId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BookingId",
                table: "Notifications",
                column: "BookingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_BookingId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BookingId",
                table: "Notifications",
                column: "BookingId");
        }
    }
}
