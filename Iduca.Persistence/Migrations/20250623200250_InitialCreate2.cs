using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iduca.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_tb_company_responsible_id",
                table: "tb_user");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_company_id",
                table: "tb_user",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_tb_company_company_id",
                table: "tb_user",
                column: "company_id",
                principalTable: "tb_company",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_tb_company_company_id",
                table: "tb_user");

            migrationBuilder.DropIndex(
                name: "IX_tb_user_company_id",
                table: "tb_user");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_tb_company_responsible_id",
                table: "tb_user",
                column: "responsible_id",
                principalTable: "tb_company",
                principalColumn: "id");
        }
    }
}
