﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SHSecurityContext.DBContext;
using System;

namespace SHSecurityContext.Migrations
{
    [DbContext(typeof(SHSecuritySysContext))]
    [Migration("20171230112229_AddPoliceGpsStaticArea")]
    partial class AddPoliceGpsStaticArea
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("SHSecurityModels.db_jjd", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("af_addr");

                    b.Property<string>("amap_gps_x");

                    b.Property<string>("amap_gps_y");

                    b.Property<string>("bjay1");

                    b.Property<string>("bjay2");

                    b.Property<string>("cjdw");

                    b.Property<string>("datetime");

                    b.Property<string>("jjdid");

                    b.Property<string>("qy");

                    b.HasKey("id");

                    b.ToTable("db_jjds");
                });

            modelBuilder.Entity("SHSecurityModels.PoliceGPS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Day");

                    b.Property<string>("GPS_X");

                    b.Property<string>("GPS_Y");

                    b.Property<string>("HH");

                    b.Property<string>("MM");

                    b.Property<string>("Month");

                    b.Property<string>("PoliceID");

                    b.Property<string>("SS");

                    b.Property<int>("Timestamp");

                    b.Property<string>("Year");

                    b.HasKey("Id");

                    b.ToTable("police_gps");
                });

            modelBuilder.Entity("SHSecurityModels.PoliceGPSAreaStatic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AreaName");

                    b.Property<string>("Day");

                    b.Property<string>("HH");

                    b.Property<string>("Month");

                    b.Property<string>("PoliceId");

                    b.Property<string>("Year");

                    b.HasKey("Id");

                    b.ToTable("PoliceGPSAreaStatic");
                });

            modelBuilder.Entity("SHSecurityModels.sys_110warningdb", b =>
                {
                    b.Property<string>("JJD_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AF_ADDR");

                    b.Property<string>("AMAP_GPS_X");

                    b.Property<string>("AMAP_GPS_Y");

                    b.Property<string>("BJAY1");

                    b.Property<string>("BJAY2");

                    b.Property<string>("BJAY3");

                    b.Property<string>("BJAY4");

                    b.Property<string>("BJAY5");

                    b.Property<string>("BJ_PHONE");

                    b.Property<string>("CJDW");

                    b.Property<string>("CJY_ID");

                    b.Property<string>("CJY_NAME");

                    b.Property<string>("COMMET");

                    b.Property<string>("DAY");

                    b.Property<string>("FKAY1");

                    b.Property<string>("FKAY2");

                    b.Property<string>("FKAY3");

                    b.Property<string>("FKAY4");

                    b.Property<string>("FKAY5");

                    b.Property<string>("HH");

                    b.Property<string>("JJY_ID");

                    b.Property<string>("JJY_NAME");

                    b.Property<string>("KYE_AREAS");

                    b.Property<string>("MM");

                    b.Property<string>("MONTH");

                    b.Property<string>("QY");

                    b.Property<string>("ROAD");

                    b.Property<string>("SS");

                    b.Property<int>("TIMESIGN")
                        .HasColumnType("bigint");

                    b.Property<string>("YEAR");

                    b.HasKey("JJD_ID");

                    b.ToTable("sys_110warningdb");
                });

            modelBuilder.Entity("SHSecurityModels.sys_cameras", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("back1");

                    b.Property<string>("back2");

                    b.Property<string>("domain");

                    b.Property<string>("lang");

                    b.Property<string>("lat");

                    b.Property<string>("name");

                    b.Property<string>("parent");

                    b.Property<string>("state");

                    b.HasKey("id");

                    b.ToTable("sys_cameras");
                });

            modelBuilder.Entity("SHSecurityModels.sys_camPeopleCount", b =>
                {
                    b.Property<string>("ID");

                    b.Property<int>("Count");

                    b.Property<int>("Time");

                    b.HasKey("ID");

                    b.ToTable("sys_camPeopleCount");
                });

            modelBuilder.Entity("SHSecurityModels.sys_config", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("key");

                    b.Property<string>("value");

                    b.Property<int>("valueInt");

                    b.HasKey("id");

                    b.ToTable("sys_config");
                });

            modelBuilder.Entity("SHSecurityModels.sys_GpsGridWarn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GridName");

                    b.Property<int>("GridX");

                    b.Property<int>("GridY");

                    b.Property<string>("JJD_ID");

                    b.Property<int>("JJD_TIMESIGN");

                    b.HasKey("Id");

                    b.ToTable("gps_grid");
                });

            modelBuilder.Entity("SHSecurityModels.sys_ticketres", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CheckTime");

                    b.Property<string>("GoDate");

                    b.Property<string>("GoLocation");

                    b.Property<string>("GoTime");

                    b.Property<string>("PassageID");

                    b.Property<string>("PassageName");

                    b.Property<string>("PassageState");

                    b.Property<string>("PassageType");

                    b.Property<string>("SeatNo");

                    b.Property<string>("TicketDate");

                    b.Property<string>("TicketTime");

                    b.Property<string>("ToLocation");

                    b.HasKey("ID");

                    b.ToTable("sys_ticketres");
                });

            modelBuilder.Entity("SHSecurityModels.sys_wifitable", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ACCESS_AP_CHANNEL");

                    b.Property<int>("ACCESS_AP_ENCRYPTION_TYPE");

                    b.Property<string>("ACCESS_AP_MAC");

                    b.Property<string>("BRAND");

                    b.Property<string>("CACHE_SSID");

                    b.Property<string>("CAPTURE_TIME");

                    b.Property<int>("CERTIFICATE_CODE");

                    b.Property<string>("COLLECTION_EQUIPMENT_ID");

                    b.Property<string>("COLLECTION_EQUIPMENT_LATITUDE");

                    b.Property<string>("COLLECTION_EQUIPMENT_LONGITUDE");

                    b.Property<int>("IDENTIFICATOIN_TYPE");

                    b.Property<string>("MAC");

                    b.Property<string>("NETBAR_WACODE");

                    b.Property<string>("SSID_POSITION");

                    b.Property<int>("TERMINAL_FIELD_STRENGTH");

                    b.Property<string>("X_COORDINATE");

                    b.Property<string>("Y_COORDINATE");

                    b.HasKey("ID");

                    b.ToTable("sys_wifitable");
                });
#pragma warning restore 612, 618
        }
    }
}
