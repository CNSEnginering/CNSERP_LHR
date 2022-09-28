using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_ICLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APSETUP1");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "GLTRH",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AprovedBy",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AprovedDate",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocId",
                table: "GLTRH",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PostedBy",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedDate",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocId",
                table: "GLTRD",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Linked",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentID",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "BKBANKS",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ICACCS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    LocID = table.Column<int>(nullable: false),
                    SegID = table.Column<string>(nullable: false),
                    AccRec = table.Column<string>(nullable: true),
                    AccRet = table.Column<string>(nullable: true),
                    AccAdj = table.Column<string>(nullable: true),
                    AccCGS = table.Column<string>(nullable: true),
                    AccWIP = table.Column<string>(nullable: true),
                    AudtUser = table.Column<string>(nullable: true),
                    AudtDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICACCS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICLOC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    LocID = table.Column<int>(nullable: false),
                    LocName = table.Column<string>(nullable: true),
                    LocShort = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    AllowRec = table.Column<bool>(nullable: false),
                    AllowNeg = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    AudtUser = table.Column<string>(nullable: true),
                    AudtDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICLOC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICSETUP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Segment1 = table.Column<string>(nullable: true),
                    Segment2 = table.Column<string>(nullable: true),
                    Segment3 = table.Column<string>(nullable: true),
                    AllowNegative = table.Column<int>(nullable: true),
                    ErrSrNo = table.Column<int>(nullable: true),
                    CostingMethod = table.Column<int>(nullable: true),
                    PRBookID = table.Column<string>(nullable: true),
                    RTBookID = table.Column<string>(nullable: true),
                    CnsBookID = table.Column<string>(nullable: true),
                    SLBookID = table.Column<string>(nullable: true),
                    SRBookID = table.Column<string>(nullable: true),
                    TRBookID = table.Column<string>(nullable: true),
                    PrdBookID = table.Column<string>(nullable: true),
                    PyRecBookID = table.Column<string>(nullable: true),
                    AdjBookID = table.Column<string>(nullable: true),
                    AsmBookID = table.Column<string>(nullable: true),
                    WSBookID = table.Column<string>(nullable: true),
                    DSBookID = table.Column<string>(nullable: true),
                    SalesReturnLinkOn = table.Column<bool>(nullable: false),
                    SalesLinkOn = table.Column<bool>(nullable: false),
                    AccLinkOn = table.Column<bool>(nullable: false),
                    CurrentLocID = table.Column<int>(nullable: true),
                    AllowLocID = table.Column<bool>(nullable: false),
                    CDateOnly = table.Column<bool>(nullable: false),
                    Opt4 = table.Column<string>(nullable: true),
                    Opt5 = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreateadOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICSETUP", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ICACCS_TenantId",
                table: "ICACCS",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ICLOC_TenantId",
                table: "ICLOC",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ICSETUP_TenantId",
                table: "ICSETUP",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ICACCS");

            migrationBuilder.DropTable(
                name: "ICLOC");

            migrationBuilder.DropTable(
                name: "ICSETUP");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "AprovedBy",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "AprovedDate",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "LocId",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "PostedBy",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "PostedDate",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "LocId",
                table: "GLTRD");

            migrationBuilder.DropColumn(
                name: "Linked",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "BKBANKS");

            migrationBuilder.CreateTable(
                name: "APSETUP1",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    DEFBANKID = table.Column<int>(nullable: true),
                    DEFCURRCODE = table.Column<int>(nullable: true),
                    DEFPAYCODE = table.Column<int>(nullable: true),
                    DEFVENCTRLACC = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APSETUP1", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APSETUP1_TenantId",
                table: "APSETUP1",
                column: "TenantId");
        }
    }
}
