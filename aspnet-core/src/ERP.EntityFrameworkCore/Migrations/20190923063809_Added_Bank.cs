using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_Bank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "APTERMS",
                newName: "TERMID");

            migrationBuilder.CreateTable(
                name: "BKBANKS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    CMPID = table.Column<string>(nullable: true),
                    BANKID = table.Column<string>(nullable: true),
                    BANKNAME = table.Column<string>(nullable: true),
                    ADDR1 = table.Column<string>(nullable: true),
                    ADDR2 = table.Column<string>(nullable: true),
                    ADDR3 = table.Column<string>(nullable: true),
                    ADDR4 = table.Column<string>(nullable: true),
                    CITY = table.Column<string>(nullable: true),
                    STATE = table.Column<string>(nullable: true),
                    COUNTRY = table.Column<string>(nullable: true),
                    POSTAL = table.Column<string>(nullable: true),
                    CONTACT = table.Column<string>(nullable: true),
                    PHONE = table.Column<string>(nullable: true),
                    FAX = table.Column<string>(nullable: true),
                    INACTIVE = table.Column<bool>(nullable: false),
                    INACTDATE = table.Column<DateTime>(nullable: true),
                    BKACCTNUMBER = table.Column<string>(nullable: true),
                    IDACCTBANK = table.Column<string>(nullable: true),
                    IDACCTWOFF = table.Column<string>(nullable: true),
                    IDACCTCRCARD = table.Column<string>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    ChartofControlId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BKBANKS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BKBANKS_GLAMF_ChartofControlId",
                        column: x => x.ChartofControlId,
                        principalTable: "GLAMF",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BKBANKS_ChartofControlId",
                table: "BKBANKS",
                column: "ChartofControlId");

            migrationBuilder.CreateIndex(
                name: "IX_BKBANKS_TenantId",
                table: "BKBANKS",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BKBANKS");

            migrationBuilder.RenameColumn(
                name: "TERMID",
                table: "APTERMS",
                newName: "Id");
        }
    }
}
