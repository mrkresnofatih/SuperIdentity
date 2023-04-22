using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    public partial class drop_userrole_clientrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRoles");

            migrationBuilder.DropTable(
                name: "UserRoleEntity");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_PrincipalName",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Clients_ClientName",
                table: "Clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_PrincipalName",
                table: "Users",
                column: "PrincipalName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Clients_ClientName",
                table: "Clients",
                column: "ClientName");

            migrationBuilder.CreateTable(
                name: "ClientRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", nullable: false),
                    ResourceName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRoles_Clients_ClientName",
                        column: x => x.ClientName,
                        principalTable: "Clients",
                        principalColumn: "ClientName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRoles_Roles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", nullable: false),
                    UserPrincipalName = table.Column<string>(type: "character varying(50)", nullable: false),
                    ResourceName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleEntity_Roles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleEntity_Users_UserPrincipalName",
                        column: x => x.UserPrincipalName,
                        principalTable: "Users",
                        principalColumn: "PrincipalName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRoles_ClientName",
                table: "ClientRoles",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRoles_RoleName",
                table: "ClientRoles",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleEntity_RoleName",
                table: "UserRoleEntity",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleEntity_UserPrincipalName",
                table: "UserRoleEntity",
                column: "UserPrincipalName");
        }
    }
}
