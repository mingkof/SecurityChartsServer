using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class addcampeople : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "sys_camPeopleCount",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hour",
                table: "sys_camPeopleCount",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Minute",
                table: "sys_camPeopleCount",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "sys_camPeopleCount",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "sys_camPeopleCount",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "sys_camPeopleCount");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "sys_camPeopleCount");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "sys_camPeopleCount");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "sys_camPeopleCount");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "sys_camPeopleCount");
        }
    }
}
