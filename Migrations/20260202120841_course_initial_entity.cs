using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class course_initial_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "course_master",
                columns: table => new
                {
                    course_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    course_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    course_description = table.Column<string>(type: "TEXT", nullable: true),
                    course_duration = table.Column<int>(type: "int", nullable: true),
                    course_fees = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    course_fees_from = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    course_fees_to = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    course_syllabus = table.Column<string>(type: "TEXT", nullable: true),
                    age_grp_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    course_is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    cover_photo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    video_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_master", x => x.course_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_master");
        }
    }
}
