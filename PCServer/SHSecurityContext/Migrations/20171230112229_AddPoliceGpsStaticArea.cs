using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class AddPoliceGpsStaticArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PoliceGPSAreaStatic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AreaName = table.Column<string>(type: "longtext", nullable: true),
                    Day = table.Column<string>(type: "longtext", nullable: true),
                    HH = table.Column<string>(type: "longtext", nullable: true),
                    Month = table.Column<string>(type: "longtext", nullable: true),
                    PoliceId = table.Column<string>(type: "longtext", nullable: true),
                    Year = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceGPSAreaStatic", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoliceGPSAreaStatic");
        }
    }
}
