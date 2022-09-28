using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_GLCONFIG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GLCONFIG",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    AccountID = table.Column<string>(nullable: false),
                    SubAccID = table.Column<int>(nullable: false),
                    ConfigID = table.Column<int>(nullable: false),
                    BookID = table.Column<string>(nullable: false),
                    PostingOn = table.Column<bool>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    GLBOOKSId = table.Column<string>(nullable: true),
                    ChartofControlId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLCONFIG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GLCONFIG_GLAMF_ChartofControlId",
                        column: x => x.ChartofControlId,
                        principalTable: "GLAMF",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GLCONFIG_GLBOOKS_GLBOOKSId",
                        column: x => x.GLBOOKSId,
                        principalTable: "GLBOOKS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLCONFIG_ChartofControlId",
                table: "GLCONFIG",
                column: "ChartofControlId");

            migrationBuilder.CreateIndex(
                name: "IX_GLCONFIG_GLBOOKSId",
                table: "GLCONFIG",
                column: "GLBOOKSId");

            migrationBuilder.CreateIndex(
                name: "IX_GLCONFIG_TenantId",
                table: "GLCONFIG",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GLCONFIG");
        }
    }
}
