using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AngularASPNETCore2WebApiAuth.Migrations
{
    public partial class Smallfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Node",
                table: "CTasks",
                newName: "Note");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Settings",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Settings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "CTasks",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "SettingID",
                table: "CTasks",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "CTasks",
                newName: "Node");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Settings",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "CTasks",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SettingID",
                table: "CTasks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
