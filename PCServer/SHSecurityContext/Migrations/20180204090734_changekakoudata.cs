using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class changekakoudata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KaKouDataJin",
                table: "KaKouDataJin");

            migrationBuilder.AlterColumn<int>(
                name: "pass_or_out",
                table: "KaKouDataJinHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "KaKouDataJinHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "pass_or_out",
                table: "KaKouDataJin",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SBBHID",
                table: "KaKouDataJin",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "KaKouDataJin",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_KaKouDataJin",
                table: "KaKouDataJin",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KaKouDataJin",
                table: "KaKouDataJin");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "KaKouDataJin");

            migrationBuilder.AlterColumn<string>(
                name: "pass_or_out",
                table: "KaKouDataJinHistory",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Count",
                table: "KaKouDataJinHistory",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "pass_or_out",
                table: "KaKouDataJin",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SBBHID",
                table: "KaKouDataJin",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_KaKouDataJin",
                table: "KaKouDataJin",
                column: "SBBHID");
        }
    }
}
