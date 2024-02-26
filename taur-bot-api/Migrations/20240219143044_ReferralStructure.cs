using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace taur_bot_api.Migrations
{
    /// <inheritdoc />
    public partial class ReferralStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferrerCode",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "ReferrerId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Investments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ReferralNodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferralId = table.Column<long>(type: "bigint", nullable: false),
                    Inline = table.Column<int>(type: "integer", nullable: false),
                    ReferrerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferralNodes_Users_ReferralId",
                        column: x => x.ReferralId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralNodes_Users_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferrerId",
                table: "Users",
                column: "ReferrerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralNodes_ReferralId",
                table: "ReferralNodes",
                column: "ReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralNodes_ReferrerId",
                table: "ReferralNodes",
                column: "ReferrerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ReferrerId",
                table: "Users",
                column: "ReferrerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ReferrerId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ReferralNodes");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReferrerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReferrerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Investments");

            migrationBuilder.AddColumn<string>(
                name: "ReferrerCode",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
