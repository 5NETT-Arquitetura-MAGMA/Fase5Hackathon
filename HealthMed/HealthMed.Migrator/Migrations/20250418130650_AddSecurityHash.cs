using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityHash",
                table: "Users");
        }
    }
}
