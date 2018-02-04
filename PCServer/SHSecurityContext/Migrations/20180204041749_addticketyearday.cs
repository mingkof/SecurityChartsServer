using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SHSecurityContext.Migrations
{
    public partial class addticketyearday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoDateDay",
                table: "sys_ticketres",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoDateMonth",
                table: "sys_ticketres",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoDateYear",
                table: "sys_ticketres",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketDay",
                table: "sys_ticketres",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketMonth",
                table: "sys_ticketres",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketYear",
                table: "sys_ticketres",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoDateDay",
                table: "sys_ticketres");

            migrationBuilder.DropColumn(
                name: "GoDateMonth",
                table: "sys_ticketres");

            migrationBuilder.DropColumn(
                name: "GoDateYear",
                table: "sys_ticketres");

            migrationBuilder.DropColumn(
                name: "TicketDay",
                table: "sys_ticketres");

            migrationBuilder.DropColumn(
                name: "TicketMonth",
                table: "sys_ticketres");

            migrationBuilder.DropColumn(
                name: "TicketYear",
                table: "sys_ticketres");
        }
    }
}
