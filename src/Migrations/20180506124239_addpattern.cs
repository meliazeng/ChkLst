using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AngularASPNETCore2WebApiAuth.Migrations
{
    public partial class addpattern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Settings",
                newName: "SettingId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CTasks",
                newName: "CTaskId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Settings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RepeatPatternId",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Node",
                table: "CTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SDate",
                table: "CTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SettingID",
                table: "CTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RepeatPatterns",
                columns: table => new
                {
                    RepeatPatternId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descr = table.Column<string>(nullable: true),
                    Func = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatPatterns", x => x.RepeatPatternId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_RepeatPatternId",
                table: "Settings",
                column: "RepeatPatternId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_RepeatPatterns_RepeatPatternId",
                table: "Settings",
                column: "RepeatPatternId",
                principalTable: "RepeatPatterns",
                principalColumn: "RepeatPatternId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_RepeatPatterns_RepeatPatternId",
                table: "Settings");

            migrationBuilder.DropTable(
                name: "RepeatPatterns");

            migrationBuilder.DropIndex(
                name: "IX_Settings_RepeatPatternId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RepeatPatternId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Node",
                table: "CTasks");

            migrationBuilder.DropColumn(
                name: "SDate",
                table: "CTasks");

            migrationBuilder.DropColumn(
                name: "SettingID",
                table: "CTasks");

            migrationBuilder.RenameColumn(
                name: "SettingId",
                table: "Settings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CTaskId",
                table: "CTasks",
                newName: "Id");
        }
    }
}
