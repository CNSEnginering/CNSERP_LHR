using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_AccountSubLedger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CSCURRATE_CSSETUP_CompanyProfileId",
                table: "CSCURRATE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE");

            migrationBuilder.DropIndex(
                name: "IX_CSCURRATE_CompanyProfileId",
                table: "CSCURRATE");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CSCURRATE");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "CSCURRATE");

            migrationBuilder.AlterColumn<string>(
                name: "CURID",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CMPID",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE",
                column: "CURID");

            migrationBuilder.CreateTable(
                name: "GLAMFSL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    AccountID = table.Column<string>(nullable: false),
                    SubAccID = table.Column<int>(nullable: false),
                    SubAccName = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    RegNo = table.Column<string>(nullable: true),
                    TAXAUTH = table.Column<string>(nullable: true),
                    ClassID = table.Column<int>(nullable: true),
                    OldSL = table.Column<string>(nullable: true),
                    LedgerType = table.Column<short>(nullable: true),
                    Agreement1 = table.Column<string>(nullable: true),
                    Agreement2 = table.Column<string>(nullable: true),
                    PayTerm = table.Column<int>(nullable: true),
                    OtherCondition = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    TenantID = table.Column<int>(nullable: false),
                    ChartofControlId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAMFSL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GLAMFSL_GLAMF_ChartofControlId",
                        column: x => x.ChartofControlId,
                        principalTable: "GLAMF",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSCURRATE_CMPID",
                table: "CSCURRATE",
                column: "CMPID");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMFSL_ChartofControlId",
                table: "GLAMFSL",
                column: "ChartofControlId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMFSL_TenantId",
                table: "GLAMFSL",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CSCURRATE_CSSETUP_CMPID",
                table: "CSCURRATE",
                column: "CMPID",
                principalTable: "CSSETUP",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CSCURRATE_CSSETUP_CMPID",
                table: "CSCURRATE");

            migrationBuilder.DropTable(
                name: "GLAMFSL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE");

            migrationBuilder.DropIndex(
                name: "IX_CSCURRATE_CMPID",
                table: "CSCURRATE");

            migrationBuilder.AlterColumn<string>(
                name: "CMPID",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CURID",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CSCURRATE",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyProfileId",
                table: "CSCURRATE",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CSCURRATE_CompanyProfileId",
                table: "CSCURRATE",
                column: "CompanyProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CSCURRATE_CSSETUP_CompanyProfileId",
                table: "CSCURRATE",
                column: "CompanyProfileId",
                principalTable: "CSSETUP",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
