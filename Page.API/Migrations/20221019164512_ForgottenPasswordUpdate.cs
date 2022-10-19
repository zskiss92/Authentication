using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Page.API.Migrations
{
    public partial class ForgottenPasswordUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordUpdated",
                table: "ForgottenPasswords",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordUpdated",
                table: "ForgottenPasswords");
        }
    }
}
