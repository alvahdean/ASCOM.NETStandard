using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EFTool.Migrations
{
    public partial class userDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeDir",
                table: "ProfileNodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProfileNodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeDir",
                table: "ProfileNodes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProfileNodes");
        }
    }
}
