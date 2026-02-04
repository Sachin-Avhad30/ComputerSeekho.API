using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class staff_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "staff_master",
                columns: table => new
                {
                    staff_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    staff_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    photo_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    staff_mobile = table.Column<string>(type: "longtext", nullable: false),
                    staff_email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    staff_username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    staff_password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    staff_role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    staff_bio = table.Column<string>(type: "TEXT", nullable: true),
                    staff_designation = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_master", x => x.staff_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_staff_master_staff_email",
                table: "staff_master",
                column: "staff_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_master_staff_username",
                table: "staff_master",
                column: "staff_username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "staff_master");
        }
    }
}
