using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EFTool.Migrations
{
    public partial class DeviceStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomeStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Altitude = table.Column<double>(nullable: false),
                    AtHome = table.Column<bool>(nullable: false),
                    AtPark = table.Column<bool>(nullable: false),
                    Azimuth = table.Column<double>(nullable: false),
                    Connected = table.Column<bool>(nullable: false),
                    ShutterStatus = table.Column<string>(nullable: true),
                    Slaved = table.Column<bool>(nullable: false),
                    Slewing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomeStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilterWheelStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Connected = table.Column<bool>(nullable: false),
                    Position = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterWheelStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FocuserStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Connected = table.Column<bool>(nullable: false),
                    Link = table.Column<bool>(nullable: false),
                    TempComp = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocuserStates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomeStates");

            migrationBuilder.DropTable(
                name: "FilterWheelStates");

            migrationBuilder.DropTable(
                name: "FocuserStates");
        }
    }
}
