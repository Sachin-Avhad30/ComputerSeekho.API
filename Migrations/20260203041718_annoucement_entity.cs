using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class annoucement_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "announcement_master",
                columns: table => new
                {
                    announcement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    announcement_text = table.Column<string>(type: "TEXT", nullable: false),
                    valid_from = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    valid_to = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcement_master", x => x.announcement_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_announcement_master_is_active",
                table: "announcement_master",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_announcement_master_valid_from_valid_to",
                table: "announcement_master",
                columns: new[] { "valid_from", "valid_to" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcement_master");
        }
    }
}
