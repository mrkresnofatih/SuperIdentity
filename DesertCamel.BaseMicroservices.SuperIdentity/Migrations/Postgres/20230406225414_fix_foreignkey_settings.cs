using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesertCamel.BaseMicroservices.SuperIdentity.Migrations.Postgres
{
    public partial class fix_foreignkey_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_RoleName",
                table: "RolePermissions");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionName",
                table: "RolePermissions",
                column: "PermissionName");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionName",
                table: "RolePermissions",
                column: "PermissionName",
                principalTable: "Permissions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionName",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_PermissionName",
                table: "RolePermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_RoleName",
                table: "RolePermissions",
                column: "RoleName",
                principalTable: "Permissions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
