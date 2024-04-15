using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Babun_API.Migrations
{
    /// <inheritdoc />
    public partial class addedsoftdeletetothedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Ads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Ads");
        }
    }
}
