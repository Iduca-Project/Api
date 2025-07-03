using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iduca.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstAccessToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_tb_course_CourseId",
                table: "Exam");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_questions_Exam_exam_id",
                table: "exam_questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam",
                table: "Exam");

            migrationBuilder.RenameTable(
                name: "Exam",
                newName: "Exams");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_CourseId",
                table: "Exams",
                newName: "IX_Exams_CourseId");

            migrationBuilder.AddColumn<bool>(
                name: "FirstAccess",
                table: "tb_user",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exams",
                table: "Exams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_exam_questions_Exams_exam_id",
                table: "exam_questions",
                column: "exam_id",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_tb_course_CourseId",
                table: "Exams",
                column: "CourseId",
                principalTable: "tb_course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exam_questions_Exams_exam_id",
                table: "exam_questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_tb_course_CourseId",
                table: "Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exams",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "FirstAccess",
                table: "tb_user");

            migrationBuilder.RenameTable(
                name: "Exams",
                newName: "Exam");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_CourseId",
                table: "Exam",
                newName: "IX_Exam_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam",
                table: "Exam",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_tb_course_CourseId",
                table: "Exam",
                column: "CourseId",
                principalTable: "tb_course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_questions_Exam_exam_id",
                table: "exam_questions",
                column: "exam_id",
                principalTable: "Exam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
