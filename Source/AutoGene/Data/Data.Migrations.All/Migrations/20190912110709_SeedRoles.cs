using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations.All.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Deleted", "Description", "Name" },
                values: new object[] { "1", false, null, "Guest" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Deleted", "Description", "Name" },
                values: new object[] { "2", false, null, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
