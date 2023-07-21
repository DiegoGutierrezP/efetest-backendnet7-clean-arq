using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfeTest.Backend.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_column_rol_to_user_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Users");
        }
    }
}
