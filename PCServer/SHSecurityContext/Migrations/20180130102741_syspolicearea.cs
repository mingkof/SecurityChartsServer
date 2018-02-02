using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class syspolicearea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckInName",
                table: "sys_ticketres",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "sys_policearea",
                columns: table => new
                {
                    AreaName = table.Column<string>(nullable: false),
                    Count = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_policearea", x => x.AreaName);
                });

            migrationBuilder.CreateTable(
                name: "sys_policeareahistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AreaName = table.Column<string>(nullable: true),
                    Count = table.Column<string>(nullable: true),
                    Day = table.Column<string>(nullable: true),
                    Hour = table.Column<string>(nullable: true),
                    Minute = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    Second = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<int>(nullable: false),
                    Year = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_policeareahistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_policearea");

            migrationBuilder.DropTable(
                name: "sys_policeareahistory");

            migrationBuilder.DropColumn(
                name: "CheckInName",
                table: "sys_ticketres");
        }
    }
}
