using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    public partial class redirecturi_callbackurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationCallbackUrl",
                table: "UserPools",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RedirectUri",
                table: "UserPools",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationCallbackUrl",
                table: "UserPools");

            migrationBuilder.DropColumn(
                name: "RedirectUri",
                table: "UserPools");
        }
    }
}
