using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taur_bot_api.Migrations
{
    /// <inheritdoc />
    public partial class IsAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HaveFirstAccrual",
                table: "Investments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "Investments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HaveFirstAccrual",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "Investments");
        }
    }
}
