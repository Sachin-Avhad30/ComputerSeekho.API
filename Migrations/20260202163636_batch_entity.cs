using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class batch_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "batch_master",
                columns: table => new
                {
                    batch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    batch_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    batch_start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    batch_end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    presentation_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    course_fees = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    course_fees_from = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    course_fees_to = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    batch_is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    final_presentation_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    batch_logo_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_master", x => x.batch_id);
                    table.ForeignKey(
                        name: "FK_batch_master_course_master_course_id",
                        column: x => x.course_id,
                        principalTable: "course_master",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_batch_master_course_id",
                table: "batch_master",
                column: "course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "batch_master");
        }
    }
}
