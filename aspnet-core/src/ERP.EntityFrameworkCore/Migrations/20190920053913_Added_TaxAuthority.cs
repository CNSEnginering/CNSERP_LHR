using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_TaxAuthority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAMFSL_GLAMF_ChartofControlId",
                table: "GLAMFSL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL");

            migrationBuilder.DropIndex(
                name: "IX_GLAMFSL_ChartofControlId",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "ChartofControlId",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "GLAMFSL");

            migrationBuilder.AlterColumn<string>(
                name: "AccountID",
                table: "GLAMFSL",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CompanyID",
                table: "CSSETUP",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CMPID",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL",
                columns: new[] { "AccountID", "SubAccID" });

            migrationBuilder.CreateTable(
                name: "CSTAXAUTH",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    CMPID = table.Column<string>(nullable: false),
                    TAXAUTH = table.Column<string>(nullable: false),
                    TAXAUTHDESC = table.Column<string>(nullable: false),
                    ACCLIABILITY = table.Column<string>(nullable: false),
                    ACCRECOVERABLE = table.Column<string>(nullable: false),
                    RECOVERRATE = table.Column<double>(nullable: false),
                    ACCEXPENSE = table.Column<string>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: false),
                    AUDTUSER = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSTAXAUTH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CSTAXAUTH_CSSETUP_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CSSETUP",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXAUTH_CompanyId",
                table: "CSTAXAUTH",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXAUTH_TenantId",
                table: "CSTAXAUTH",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSTAXAUTH");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL");

            migrationBuilder.AlterColumn<string>(
                name: "AccountID",
                table: "GLAMFSL",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GLAMFSL",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ChartofControlId",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantID",
                table: "GLAMFSL",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyID",
                table: "CSSETUP",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CMPID",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMFSL_ChartofControlId",
                table: "GLAMFSL",
                column: "ChartofControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMFSL_GLAMF_ChartofControlId",
                table: "GLAMFSL",
                column: "ChartofControlId",
                principalTable: "GLAMF",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
