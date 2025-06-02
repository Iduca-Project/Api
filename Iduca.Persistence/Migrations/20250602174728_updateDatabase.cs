using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iduca.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_course_tb_exam_exam_id",
                table: "tb_course");

            migrationBuilder.DropTable(
                name: "exam_questions");

            migrationBuilder.DropTable(
                name: "exercise_questions");

            migrationBuilder.DropTable(
                name: "tb_exam");

            migrationBuilder.DropIndex(
                name: "IX_tb_course_exam_id",
                table: "tb_course");

            migrationBuilder.DropColumn(
                name: "exam_id",
                table: "tb_course");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "tb_exercise",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_tb_exercise_QuestionId",
                table: "tb_exercise",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_exercise_tb_question_QuestionId",
                table: "tb_exercise",
                column: "QuestionId",
                principalTable: "tb_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_exercise_tb_question_QuestionId",
                table: "tb_exercise");

            migrationBuilder.DropIndex(
                name: "IX_tb_exercise_QuestionId",
                table: "tb_exercise");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "tb_exercise");

            migrationBuilder.AddColumn<Guid>(
                name: "exam_id",
                table: "tb_course",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "exercise_questions",
                columns: table => new
                {
                    exercise_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    question_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_questions", x => new { x.exercise_id, x.question_id });
                    table.ForeignKey(
                        name: "FK_exercise_questions_tb_exercise_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "tb_exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exercise_questions_tb_question_question_id",
                        column: x => x.question_id,
                        principalTable: "tb_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_exam",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    course_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    description = table.Column<string>(type: "varchar(511)", maxLength: 511, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    disabled_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("exam_id", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exam_questions",
                columns: table => new
                {
                    exam_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    question_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_questions", x => new { x.exam_id, x.question_id });
                    table.ForeignKey(
                        name: "FK_exam_questions_tb_exam_exam_id",
                        column: x => x.exam_id,
                        principalTable: "tb_exam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_questions_tb_question_question_id",
                        column: x => x.question_id,
                        principalTable: "tb_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_course_exam_id",
                table: "tb_course",
                column: "exam_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_question_id",
                table: "exam_questions",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_questions_question_id",
                table: "exercise_questions",
                column: "question_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_course_tb_exam_exam_id",
                table: "tb_course",
                column: "exam_id",
                principalTable: "tb_exam",
                principalColumn: "id");
        }
    }
}
