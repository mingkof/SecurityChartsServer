using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class mod201801241843 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "HongWaiPeopleData",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "HongWaiPeopleData",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "HongWaiPeopleData",
                type: "longtext",
                nullable: true);


            migrationBuilder.CreateTable(
                name: "PeopleCountConfig",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(127)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleCountConfig", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeopleCountConfig");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "HongWaiPeopleData");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "HongWaiPeopleData");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "HongWaiPeopleData");

            migrationBuilder.DropColumn(
                name: "cameraName",
                table: "FaceAlarmData");

            migrationBuilder.DropColumn(
                name: "humanId",
                table: "FaceAlarmData");

            migrationBuilder.DropColumn(
                name: "humanName",
                table: "FaceAlarmData");

            migrationBuilder.DropColumn(
                name: "matchHumanList",
                table: "FaceAlarmData");

            migrationBuilder.AddColumn<string>(
                name: "cameraId",
                table: "FaceAlarmData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "facePicUrl",
                table: "FaceAlarmData",
                nullable: true);
        }
    }
}
