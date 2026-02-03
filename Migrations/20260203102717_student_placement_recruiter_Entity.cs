using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class student_placement_recruiter_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recruiter_Master",
                columns: table => new
                {
                    recruiter_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    recruiter_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    logo_url = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruiter_Master", x => x.recruiter_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_master",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    batch_id = table.Column<int>(type: "int", nullable: true),
                    course_id = table.Column<int>(type: "int", nullable: true),
                    student_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    student_mobile = table.Column<long>(type: "bigint", nullable: false),
                    student_gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    student_dob = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    student_address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    student_qualification = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    student_username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    student_password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    photo_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    registration_status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_master", x => x.student_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Placements_Master",
                columns: table => new
                {
                    placement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    batch_id = table.Column<int>(type: "int", nullable: false),
                    recruiter_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placements_Master", x => x.placement_id);
                    table.ForeignKey(
                        name: "FK_Placements_Master_Recruiter_Master_recruiter_id",
                        column: x => x.recruiter_id,
                        principalTable: "Recruiter_Master",
                        principalColumn: "recruiter_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_Master_batch_master_batch_id",
                        column: x => x.batch_id,
                        principalTable: "batch_master",
                        principalColumn: "batch_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_Master_student_master_student_id",
                        column: x => x.student_id,
                        principalTable: "student_master",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_Master_batch_id",
                table: "Placements_Master",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_Master_recruiter_id",
                table: "Placements_Master",
                column: "recruiter_id");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_Master_student_id",
                table: "Placements_Master",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Placements_Master");

            migrationBuilder.DropTable(
                name: "Recruiter_Master");

            migrationBuilder.DropTable(
                name: "student_master");
        }
    }
}
