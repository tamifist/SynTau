using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations.All.Migrations
{
    public partial class Common_BaseEntity_CreatedAtDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Roles",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "GeneOrders",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Roles",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "GeneOrders",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true,
                oldDefaultValueSql: "getdate()");
        }
    }
}
