using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_APTerm : Migration
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
                name: "APTERMS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    TERMID = table.Column<int>(nullable: false),
                    TERMDESC = table.Column<string>(nullable: true),
                    TERMRATE = table.Column<double>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    INACTIVE = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APTERMS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APTERMS_TenantId",
                table: "APTERMS",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APTERMS");

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
