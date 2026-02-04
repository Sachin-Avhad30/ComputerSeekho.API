using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class all_entityrebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "album_master",
                columns: table => new
                {
                    album_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    album_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    album_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    album_is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_master", x => x.album_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "closure_reason_master",
                columns: table => new
                {
                    closure_reason_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    closure_reason_desc = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_closure_reason_master", x => x.closure_reason_id);
                })
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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_master", x => x.staff_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "image_master",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    image_path = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    album_id = table.Column<int>(type: "int", nullable: false),
                    is_album_cover = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    image_is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image_master", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_image_master_album_master_album_id",
                        column: x => x.album_id,
                        principalTable: "album_master",
                        principalColumn: "album_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_master", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_student_master_batch_master_batch_id",
                        column: x => x.batch_id,
                        principalTable: "batch_master",
                        principalColumn: "batch_id");
                    table.ForeignKey(
                        name: "FK_student_master_course_master_course_id",
                        column: x => x.course_id,
                        principalTable: "course_master",
                        principalColumn: "course_id");
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
                name: "IX_announcement_master_is_active",
                table: "announcement_master",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_announcement_master_valid_from_valid_to",
                table: "announcement_master",
                columns: new[] { "valid_from", "valid_to" });

            migrationBuilder.CreateIndex(
                name: "IX_batch_master_course_id",
                table: "batch_master",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_master_album_id",
                table: "image_master",
                column: "album_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_student_master_batch_id",
                table: "student_master",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_master_course_id",
                table: "student_master",
                column: "course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcement_master");

            migrationBuilder.DropTable(
                name: "closure_reason_master");

            migrationBuilder.DropTable(
                name: "image_master");

            migrationBuilder.DropTable(
                name: "Placements_Master");

            migrationBuilder.DropTable(
                name: "staff_master");

            migrationBuilder.DropTable(
                name: "album_master");

            migrationBuilder.DropTable(
                name: "Recruiter_Master");

            migrationBuilder.DropTable(
                name: "student_master");

            migrationBuilder.DropTable(
                name: "batch_master");

            migrationBuilder.DropTable(
                name: "course_master");
        }
    }
}
