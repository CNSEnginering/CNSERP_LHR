using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_CompanyProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CSSETUP",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    CompanyID = table.Column<string>(maxLength: 10, nullable: false),
                    CompanyName = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    SLRegNo = table.Column<string>(nullable: true),
                    FiscalStartDate = table.Column<DateTime>(nullable: true),
                    FiscalEndDate = table.Column<DateTime>(nullable: true),
                    CONTPERSON = table.Column<string>(nullable: true),
                    CONTPHONE = table.Column<string>(nullable: true),
                    CONTEMAIL = table.Column<string>(nullable: true),
                    DbID = table.Column<int>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    UseLocID = table.Column<bool>(nullable: false),
                    DirectPost = table.Column<bool>(nullable: false),
                    LocDate = table.Column<DateTime>(nullable: true),
                    Alertmsg = table.Column<string>(nullable: true),
                    AppDate = table.Column<DateTime>(nullable: true),
                    AppSerial = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSSETUP", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSSETUP_TenantId",
                table: "CSSETUP",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSSETUP");
        }
    }
}
