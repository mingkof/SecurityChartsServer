using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class mqServerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MQServerData",
                columns: table => new
                {
                    key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    commRmk = table.Column<string>(type: "longtext", nullable: true),
                    dsnum = table.Column<string>(type: "longtext", nullable: true),
                    foreignld = table.Column<string>(type: "longtext", nullable: true),
                    mtype = table.Column<string>(type: "longtext", nullable: true),
                    projectId = table.Column<string>(type: "longtext", nullable: true),
                    time = table.Column<string>(type: "longtext", nullable: true),
                    timeStamp = table.Column<int>(type: "int", nullable: false),
                    topicType = table.Column<string>(type: "longtext", nullable: true),
                    userId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MQServerData", x => x.key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MQServerData");
        }
    }
}
