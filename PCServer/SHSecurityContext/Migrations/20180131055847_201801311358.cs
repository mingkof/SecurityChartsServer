using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class _201801311358 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarAlarmData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Day = table.Column<string>(type: "longtext", nullable: true),
                    Month = table.Column<string>(type: "longtext", nullable: true),
                    Positon = table.Column<string>(type: "longtext", nullable: true),
                    Year = table.Column<string>(type: "longtext", nullable: true),
                    alarmTime = table.Column<string>(type: "longtext", nullable: true),
                    plateId = table.Column<string>(type: "longtext", nullable: true),
                    timeStamp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarAlarmData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarAlarmData");
        }
    }
}
