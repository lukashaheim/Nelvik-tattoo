using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nelvik_Tattoo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotesFromBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Bookings",
                type: "TEXT",
                nullable: true);
        }
    }
}
