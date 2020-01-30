using Microsoft.EntityFrameworkCore.Migrations;

namespace HirePros.Migrations
{
    public partial class AddSendMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserMessage",
                table: "UserProfs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserMessage",
                table: "UserProfs");
        }
    }
}
