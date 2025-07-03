using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iduca.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstAccessToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FirstAccess",
                table: "tb_user",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstAccess",
                table: "tb_user");
        }
    }
}
