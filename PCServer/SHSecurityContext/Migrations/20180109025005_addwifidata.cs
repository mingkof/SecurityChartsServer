using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class addwifidata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WifiDataPeoples",
                columns: table => new
                {
                    WifiID = table.Column<string>(nullable: false),
                    AreaId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Timestamp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WifiDataPeoples", x => x.WifiID);
                });

            migrationBuilder.CreateTable(
                name: "WifiDataPeoplesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Count = table.Column<int>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    HH = table.Column<string>(nullable: true),
                    MM = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    SS = table.Column<string>(nullable: true),
                    Timestamp = table.Column<int>(nullable: false),
                    WifiID = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WifiDataPeoplesHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WifiDataPeoples");

            migrationBuilder.DropTable(
                name: "WifiDataPeoplesHistory");
        }
    }
}
