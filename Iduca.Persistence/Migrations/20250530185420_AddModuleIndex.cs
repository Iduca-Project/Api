using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iduca.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "tb_module",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "tb_module");
        }
    }
}
