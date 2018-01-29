using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class facealarmdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "FaceAlarmData",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "FaceAlarmData",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "FaceAlarmData",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "FaceAlarmData");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "FaceAlarmData");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "FaceAlarmData");
        }
    }
}
