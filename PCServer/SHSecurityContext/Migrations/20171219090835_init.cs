using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "db_jjds",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    af_addr = table.Column<string>(type: "longtext", nullable: true),
                    amap_gps_x = table.Column<string>(type: "longtext", nullable: true),
                    amap_gps_y = table.Column<string>(type: "longtext", nullable: true),
                    bjay1 = table.Column<string>(type: "longtext", nullable: true),
                    bjay2 = table.Column<string>(type: "longtext", nullable: true),
                    cjdw = table.Column<string>(type: "longtext", nullable: true),
                    datetime = table.Column<string>(type: "longtext", nullable: true),
                    jjdid = table.Column<string>(type: "longtext", nullable: true),
                    qy = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_jjds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gps_grid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GridName = table.Column<string>(type: "longtext", nullable: true),
                    GridX = table.Column<int>(type: "int", nullable: false),
                    GridY = table.Column<int>(type: "int", nullable: false),
                    JJD_ID = table.Column<string>(type: "longtext", nullable: true),
                    JJD_TIMESIGN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gps_grid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "police_gps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GPS_X = table.Column<string>(type: "longtext", nullable: true),
                    GPS_Y = table.Column<string>(type: "longtext", nullable: true),
                    PoliceID = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_police_gps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_110warningdb",
                columns: table => new
                {
                    JJD_ID = table.Column<string>(type: "varchar(127)", nullable: false),
                    AF_ADDR = table.Column<string>(type: "longtext", nullable: true),
                    AMAP_GPS_X = table.Column<string>(type: "longtext", nullable: true),
                    AMAP_GPS_Y = table.Column<string>(type: "longtext", nullable: true),
                    BJAY1 = table.Column<string>(type: "longtext", nullable: true),
                    BJAY2 = table.Column<string>(type: "longtext", nullable: true),
                    BJAY3 = table.Column<string>(type: "longtext", nullable: true),
                    BJAY4 = table.Column<string>(type: "longtext", nullable: true),
                    BJAY5 = table.Column<string>(type: "longtext", nullable: true),
                    BJ_PHONE = table.Column<string>(type: "longtext", nullable: true),
                    CJDW = table.Column<string>(type: "longtext", nullable: true),
                    CJY_ID = table.Column<string>(type: "longtext", nullable: true),
                    CJY_NAME = table.Column<string>(type: "longtext", nullable: true),
                    COMMET = table.Column<string>(type: "longtext", nullable: true),
                    DAY = table.Column<string>(type: "longtext", nullable: true),
                    FKAY1 = table.Column<string>(type: "longtext", nullable: true),
                    FKAY2 = table.Column<string>(type: "longtext", nullable: true),
                    FKAY3 = table.Column<string>(type: "longtext", nullable: true),
                    FKAY4 = table.Column<string>(type: "longtext", nullable: true),
                    FKAY5 = table.Column<string>(type: "longtext", nullable: true),
                    HH = table.Column<string>(type: "longtext", nullable: true),
                    JJY_ID = table.Column<string>(type: "longtext", nullable: true),
                    JJY_NAME = table.Column<string>(type: "longtext", nullable: true),
                    KYE_AREAS = table.Column<string>(type: "longtext", nullable: true),
                    MM = table.Column<string>(type: "longtext", nullable: true),
                    MONTH = table.Column<string>(type: "longtext", nullable: true),
                    QY = table.Column<string>(type: "longtext", nullable: true),
                    ROAD = table.Column<string>(type: "longtext", nullable: true),
                    SS = table.Column<string>(type: "longtext", nullable: true),
                    TIMESIGN = table.Column<int>(type: "bigint", nullable: false),
                    YEAR = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_110warningdb", x => x.JJD_ID);
                });

            migrationBuilder.CreateTable(
                name: "sys_cameras",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(127)", nullable: false),
                    back1 = table.Column<string>(type: "longtext", nullable: true),
                    back2 = table.Column<string>(type: "longtext", nullable: true),
                    domain = table.Column<string>(type: "longtext", nullable: true),
                    lang = table.Column<string>(type: "longtext", nullable: true),
                    lat = table.Column<string>(type: "longtext", nullable: true),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    parent = table.Column<string>(type: "longtext", nullable: true),
                    state = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_cameras", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    key = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "longtext", nullable: true),
                    valueInt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_ticketres",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CheckTime = table.Column<string>(type: "longtext", nullable: true),
                    GoDate = table.Column<string>(type: "longtext", nullable: true),
                    GoLocation = table.Column<string>(type: "longtext", nullable: true),
                    GoTime = table.Column<string>(type: "longtext", nullable: true),
                    PassageID = table.Column<string>(type: "longtext", nullable: true),
                    PassageName = table.Column<string>(type: "longtext", nullable: true),
                    PassageState = table.Column<string>(type: "longtext", nullable: true),
                    PassageType = table.Column<string>(type: "longtext", nullable: true),
                    SeatNo = table.Column<string>(type: "longtext", nullable: true),
                    TicketDate = table.Column<string>(type: "longtext", nullable: true),
                    TicketTime = table.Column<string>(type: "longtext", nullable: true),
                    ToLocation = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_ticketres", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "sys_wifitable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ACCESS_AP_CHANNEL = table.Column<int>(type: "int", nullable: false),
                    ACCESS_AP_ENCRYPTION_TYPE = table.Column<int>(type: "int", nullable: false),
                    ACCESS_AP_MAC = table.Column<string>(type: "longtext", nullable: true),
                    BRAND = table.Column<string>(type: "longtext", nullable: true),
                    CACHE_SSID = table.Column<string>(type: "longtext", nullable: true),
                    CAPTURE_TIME = table.Column<string>(type: "longtext", nullable: true),
                    CERTIFICATE_CODE = table.Column<int>(type: "int", nullable: false),
                    COLLECTION_EQUIPMENT_ID = table.Column<string>(type: "longtext", nullable: true),
                    COLLECTION_EQUIPMENT_LATITUDE = table.Column<string>(type: "longtext", nullable: true),
                    COLLECTION_EQUIPMENT_LONGITUDE = table.Column<string>(type: "longtext", nullable: true),
                    IDENTIFICATOIN_TYPE = table.Column<int>(type: "int", nullable: false),
                    MAC = table.Column<string>(type: "longtext", nullable: true),
                    NETBAR_WACODE = table.Column<string>(type: "longtext", nullable: true),
                    SSID_POSITION = table.Column<string>(type: "longtext", nullable: true),
                    TERMINAL_FIELD_STRENGTH = table.Column<int>(type: "int", nullable: false),
                    X_COORDINATE = table.Column<string>(type: "longtext", nullable: true),
                    Y_COORDINATE = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_wifitable", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "db_jjds");

            migrationBuilder.DropTable(
                name: "gps_grid");

            migrationBuilder.DropTable(
                name: "police_gps");

            migrationBuilder.DropTable(
                name: "sys_110warningdb");

            migrationBuilder.DropTable(
                name: "sys_cameras");

            migrationBuilder.DropTable(
                name: "sys_config");

            migrationBuilder.DropTable(
                name: "sys_ticketres");

            migrationBuilder.DropTable(
                name: "sys_wifitable");
        }
    }
}
