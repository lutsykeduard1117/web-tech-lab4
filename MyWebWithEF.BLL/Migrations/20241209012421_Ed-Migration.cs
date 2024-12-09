using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebWithEF.BLL.Migrations
{
    /// <inheritdoc />
    public partial class EdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Posts");
        }
    }
}
