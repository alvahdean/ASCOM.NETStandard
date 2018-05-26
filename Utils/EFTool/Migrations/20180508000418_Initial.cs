using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EFTool.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileNodes",
                columns: table => new
                {
                    ProfileNodeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ParentProfileNodeId = table.Column<int>(nullable: true),
                    ProfileType = table.Column<string>(nullable: false),
                    Hostname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileNodes", x => x.ProfileNodeId);
                    table.ForeignKey(
                        name: "FK_ProfileNodes_ProfileNodes_ParentProfileNodeId",
                        column: x => x.ParentProfileNodeId,
                        principalTable: "ProfileNodes",
                        principalColumn: "ProfileNodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimpleValues",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleValues", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ProfileValues",
                columns: table => new
                {
                    ProfileValueId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    ParentProfileNodeId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileValues", x => x.ProfileValueId);
                    table.ForeignKey(
                        name: "FK_ProfileValues_ProfileNodes_ParentProfileNodeId",
                        column: x => x.ParentProfileNodeId,
                        principalTable: "ProfileNodes",
                        principalColumn: "ProfileNodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileNodes_ParentProfileNodeId_Name",
                table: "ProfileNodes",
                columns: new[] { "ParentProfileNodeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileValues_ParentProfileNodeId_Key",
                table: "ProfileValues",
                columns: new[] { "ParentProfileNodeId", "Key" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileValues");

            migrationBuilder.DropTable(
                name: "SimpleValues");

            migrationBuilder.DropTable(
                name: "ProfileNodes");
        }
    }
}
