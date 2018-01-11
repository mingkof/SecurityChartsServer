using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class roaddatarecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoadDataRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Day = table.Column<string>(type: "longtext", nullable: true),
                    HH = table.Column<string>(type: "longtext", nullable: true),
                    MM = table.Column<string>(type: "longtext", nullable: true),
                    Month = table.Column<string>(type: "longtext", nullable: true),
                    Roadname = table.Column<string>(type: "longtext", nullable: true),
                    Timestamp = table.Column<int>(type: "int", nullable: false),
                    TrafficAvgSpeed = table.Column<string>(type: "longtext", nullable: true),
                    TrafficData = table.Column<string>(type: "longtext", nullable: true),
                    Year = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadDataRecord", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoadDataRecord");
        }
    }
}
