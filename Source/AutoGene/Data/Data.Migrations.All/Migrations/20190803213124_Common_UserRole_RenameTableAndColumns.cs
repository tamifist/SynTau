using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations.All.Migrations
{
    public partial class Common_UserRole_RenameTableAndColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUsers_Roles_Role_Id",
                table: "RoleUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUsers_Users_User_Id",
                table: "RoleUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUsers",
                table: "RoleUsers");

            migrationBuilder.RenameTable(
                name: "RoleUsers",
                newName: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "Role_Id",
                table: "UserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "UserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUsers_Role_Id",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "RoleUsers");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RoleUsers",
                newName: "Role_Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RoleUsers",
                newName: "User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "RoleUsers",
                newName: "IX_RoleUsers_Role_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUsers",
                table: "RoleUsers",
                columns: new[] { "User_Id", "Role_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUsers_Roles_Role_Id",
                table: "RoleUsers",
                column: "Role_Id",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUsers_Users_User_Id",
                table: "RoleUsers",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
