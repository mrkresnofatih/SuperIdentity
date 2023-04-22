using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    public partial class init_clientauthority_tb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Clients_ClientName",
                table: "Clients",
                column: "ClientName");

            migrationBuilder.CreateTable(
                name: "ClientAuthorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientName = table.Column<string>(type: "character varying(50)", nullable: false),
                    RoleResourceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAuthorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAuthorities_Clients_ClientName",
                        column: x => x.ClientName,
                        principalTable: "Clients",
                        principalColumn: "ClientName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAuthorities_RoleResources_RoleResourceId",
                        column: x => x.RoleResourceId,
                        principalTable: "RoleResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientAuthorities_ClientName",
                table: "ClientAuthorities",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAuthorities_RoleResourceId",
                table: "ClientAuthorities",
                column: "RoleResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientAuthorities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Clients_ClientName",
                table: "Clients");
        }
    }
}
