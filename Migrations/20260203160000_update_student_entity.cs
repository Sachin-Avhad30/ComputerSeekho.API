using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSeekho.API.Migrations
{
    /// <inheritdoc />
    public partial class update_student_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "student_master",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "student_master",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_master_batch_id",
                table: "student_master",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_master_course_id",
                table: "student_master",
                column: "course_id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_master_batch_master_batch_id",
                table: "student_master",
                column: "batch_id",
                principalTable: "batch_master",
                principalColumn: "batch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_master_course_master_course_id",
                table: "student_master",
                column: "course_id",
                principalTable: "course_master",
                principalColumn: "course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_master_batch_master_batch_id",
                table: "student_master");

            migrationBuilder.DropForeignKey(
                name: "FK_student_master_course_master_course_id",
                table: "student_master");

            migrationBuilder.DropIndex(
                name: "IX_student_master_batch_id",
                table: "student_master");

            migrationBuilder.DropIndex(
                name: "IX_student_master_course_id",
                table: "student_master");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "student_master",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "student_master",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
