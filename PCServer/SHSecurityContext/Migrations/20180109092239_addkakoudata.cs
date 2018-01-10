using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class addkakoudata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KaKouDataJin",
                columns: table => new
                {
                    SBBHID = table.Column<string>(type: "varchar(127)", nullable: false),
                    Count = table.Column<string>(type: "longtext", nullable: true),
                    SBMC = table.Column<string>(type: "longtext", nullable: true),
                    Timestamp = table.Column<int>(type: "int", nullable: false),
                    XSFX = table.Column<string>(type: "longtext", nullable: true),
                    pass_or_out = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaKouDataJin", x => x.SBBHID);
                });

            migrationBuilder.CreateTable(
                name: "KaKouDataJinHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Count = table.Column<string>(type: "longtext", nullable: true),
                    Day = table.Column<string>(type: "longtext", nullable: true),
                    HH = table.Column<string>(type: "longtext", nullable: true),
                    Month = table.Column<string>(type: "longtext", nullable: true),
                    SBBHID = table.Column<string>(type: "longtext", nullable: true),
                    SBMC = table.Column<string>(type: "longtext", nullable: true),
                    XSFX = table.Column<string>(type: "longtext", nullable: true),
                    Year = table.Column<string>(type: "longtext", nullable: true),
                    pass_or_out = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaKouDataJinHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KaKouDataJin");

            migrationBuilder.DropTable(
                name: "KaKouDataJinHistory");
        }
    }
}
