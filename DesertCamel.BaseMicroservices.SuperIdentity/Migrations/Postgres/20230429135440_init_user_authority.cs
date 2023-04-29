using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    public partial class init_user_authority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_PrincipalName",
                table: "Users",
                column: "PrincipalName");

            migrationBuilder.CreateTable(
                name: "UserAuthorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrincipalName = table.Column<string>(type: "character varying(50)", nullable: false),
                    RoleResourceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAuthorities_RoleResources_RoleResourceId",
                        column: x => x.RoleResourceId,
                        principalTable: "RoleResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAuthorities_Users_PrincipalName",
                        column: x => x.PrincipalName,
                        principalTable: "Users",
                        principalColumn: "PrincipalName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorities_PrincipalName",
                table: "UserAuthorities",
                column: "PrincipalName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorities_RoleResourceId",
                table: "UserAuthorities",
                column: "RoleResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthorities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_PrincipalName",
                table: "Users");
        }
    }
}
