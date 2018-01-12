using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class modcameras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "worldX",
                table: "sys_cameras",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "worldY",
                table: "sys_cameras",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "worldX",
                table: "sys_cameras");

            migrationBuilder.DropColumn(
                name: "worldY",
                table: "sys_cameras");
        }
    }
}
