using Microsoft.EntityFrameworkCore.Migrations;

namespace ZealandDimselab.Migrations
{
    public partial class RemovedBookingItemQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BookingItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BookingItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
