using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Regenerated_Bank6038 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GLBooks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GLBooks",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    BookID = table.Column<string>(nullable: false),
                    BookName = table.Column<string>(nullable: false),
                    INACTIVE = table.Column<bool>(nullable: false),
                    Integrated = table.Column<bool>(nullable: false),
                    NormalEntry = table.Column<int>(nullable: false),
                    Restricted = table.Column<short>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLBooks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLBooks_TenantId",
                table: "GLBooks",
                column: "TenantId");
        }
    }
}
