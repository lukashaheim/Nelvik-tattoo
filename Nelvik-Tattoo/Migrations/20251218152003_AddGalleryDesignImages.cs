using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nelvik_Tattoo.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryDesignImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "GalleryDesigns",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Placement",
                table: "GalleryDesigns",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GalleryDesignImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GalleryDesignId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryDesignImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GalleryDesignImages_GalleryDesigns_GalleryDesignId",
                        column: x => x.GalleryDesignId,
                        principalTable: "GalleryDesigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryDesignImages_GalleryDesignId",
                table: "GalleryDesignImages",
                column: "GalleryDesignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryDesignImages");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "GalleryDesigns");

            migrationBuilder.DropColumn(
                name: "Placement",
                table: "GalleryDesigns");
        }
    }
}
