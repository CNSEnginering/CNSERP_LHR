using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_CurrencyRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CSCURRATE",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    CMPID = table.Column<string>(nullable: false),
                    CURID = table.Column<string>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: false),
                    AUDTUSER = table.Column<string>(nullable: false),
                    CURNAME = table.Column<string>(nullable: false),
                    SYMBOL = table.Column<string>(nullable: false),
                    RATEDATE = table.Column<DateTime>(nullable: false),
                    CURRATE = table.Column<double>(nullable: false),
                    CompanyProfileId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSCURRATE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CSCURRATE_CSSETUP_CompanyProfileId",
                        column: x => x.CompanyProfileId,
                        principalTable: "CSSETUP",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSCURRATE_CompanyProfileId",
                table: "CSCURRATE",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CSCURRATE_TenantId",
                table: "CSCURRATE",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSCURRATE");
        }
    }
}
