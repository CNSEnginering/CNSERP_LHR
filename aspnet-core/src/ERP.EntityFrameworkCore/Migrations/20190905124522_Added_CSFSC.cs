using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_CSFSC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CSSETUP",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CSSETUP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSSETUP",
                table: "CSSETUP",
                column: "CompanyID");

            migrationBuilder.CreateTable(
                name: "CSFSC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    FSCYEAR = table.Column<int>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: false),
                    AUDTUSER = table.Column<string>(nullable: true),
                    PERIODS = table.Column<short>(nullable: false),
                    QTR4PERD = table.Column<short>(nullable: false),
                    ACTIVE = table.Column<short>(nullable: false),
                    BGNDATE1 = table.Column<DateTime>(nullable: false),
                    BGNDATE2 = table.Column<DateTime>(nullable: false),
                    BGNDATE3 = table.Column<DateTime>(nullable: false),
                    BGNDATE4 = table.Column<DateTime>(nullable: false),
                    BGNDATE5 = table.Column<DateTime>(nullable: false),
                    BGNDATE6 = table.Column<DateTime>(nullable: false),
                    BGNDATE7 = table.Column<DateTime>(nullable: false),
                    BGNDATE8 = table.Column<DateTime>(nullable: false),
                    BGNDATE9 = table.Column<DateTime>(nullable: false),
                    BGNDATE10 = table.Column<DateTime>(nullable: false),
                    BGNDATE11 = table.Column<DateTime>(nullable: false),
                    BGNDATE12 = table.Column<DateTime>(nullable: false),
                    BGNDATE13 = table.Column<DateTime>(nullable: false),
                    ENDDATE1 = table.Column<DateTime>(nullable: false),
                    ENDDATE2 = table.Column<DateTime>(nullable: false),
                    ENDDATE3 = table.Column<DateTime>(nullable: false),
                    ENDDATE4 = table.Column<DateTime>(nullable: false),
                    ENDDATE5 = table.Column<DateTime>(nullable: false),
                    ENDDATE6 = table.Column<DateTime>(nullable: false),
                    ENDDATE7 = table.Column<DateTime>(nullable: false),
                    ENDDATE8 = table.Column<DateTime>(nullable: false),
                    ENDDATE9 = table.Column<DateTime>(nullable: false),
                    ENDDATE10 = table.Column<DateTime>(nullable: false),
                    ENDDATE11 = table.Column<DateTime>(nullable: false),
                    ENDDATE12 = table.Column<DateTime>(nullable: false),
                    ENDDATE13 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSFSC", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSFSC_TenantId",
                table: "CSFSC",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSFSC");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSSETUP",
                table: "CSSETUP");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CSSETUP",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSSETUP",
                table: "CSSETUP",
                column: "Id");
        }
    }
}
