using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class facealarmdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaceAlarmData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    alarmId = table.Column<string>(type: "longtext", nullable: true),
                    alarmTime = table.Column<string>(type: "longtext", nullable: true),
                    cameraName = table.Column<string>(type: "longtext", nullable: true),
                    position = table.Column<string>(type: "longtext", nullable: true),
                    timeStamp = table.Column<int>(type: "int", nullable: false),
                    humanId = table.Column<string>(type: "longtext", nullable: true),
                    humanName = table.Column<string>(type: "longtext", nullable: true),
                    matchHumanList = table.Column<string>(type: "longtext", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceAlarmData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaceAlarmData");
        }
    }
}
