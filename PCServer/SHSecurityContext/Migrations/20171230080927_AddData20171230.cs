using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class AddData20171230 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "police_gps",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HH",
                table: "police_gps",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MM",
                table: "police_gps",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "police_gps",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SS",
                table: "police_gps",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Timestamp",
                table: "police_gps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "police_gps",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "police_gps");

            migrationBuilder.DropColumn(
                name: "HH",
                table: "police_gps");

            migrationBuilder.DropColumn(
                name: "MM",
                table: "police_gps");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "police_gps");

            migrationBuilder.DropColumn(
                name: "SS",
                table: "police_gps");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "police_gps");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "police_gps");
        }
    }
}
