using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    public partial class init_clientrole_tb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Clients_ClientName",
                table: "Clients",
                column: "ClientName");

            migrationBuilder.CreateTable(
                name: "ClientRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", nullable: false),
                    ClientName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_ClientRoles_ClientName",
                table: "ClientRoles",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRoles_RoleName",
                table: "ClientRoles",
                column: "RoleName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRoles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Clients_ClientName",
                table: "Clients");
        }
    }
}
