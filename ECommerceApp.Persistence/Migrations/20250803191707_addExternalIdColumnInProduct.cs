using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addExternalIdColumnInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalProductId",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalProductId",
                table: "Products");
        }
    }
}
