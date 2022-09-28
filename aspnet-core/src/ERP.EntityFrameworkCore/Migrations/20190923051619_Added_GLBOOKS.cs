using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_GLBOOKS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CSTAXAUTH_CSSETUP_CompanyId",
                table: "CSTAXAUTH");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSTAXAUTH",
                table: "CSTAXAUTH");

            migrationBuilder.DropIndex(
                name: "IX_CSTAXAUTH_CompanyId",
                table: "CSTAXAUTH");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CSTAXAUTH");

            migrationBuilder.AlterColumn<string>(
                name: "TAXAUTH",
                table: "CSTAXAUTH",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "CSTAXAUTH",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CMPID",
                table: "CSTAXAUTH",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSTAXAUTH",
                table: "CSTAXAUTH",
                columns: new[] { "CompanyId", "TAXAUTH" });

            migrationBuilder.CreateTable(
                name: "GLBOOKS",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    BookID = table.Column<string>(nullable: false),
                    BookName = table.Column<string>(nullable: false),
                    NormalEntry = table.Column<int>(nullable: false),
                    Integrated = table.Column<bool>(nullable: false),
                    INACTIVE = table.Column<bool>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true),
                    Restricted = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLBOOKS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLBOOKS_TenantId",
                table: "GLBOOKS",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CSTAXAUTH_CSSETUP_CompanyId",
                table: "CSTAXAUTH",
                column: "CompanyId",
                principalTable: "CSSETUP",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CSTAXAUTH_CSSETUP_CompanyId",
                table: "CSTAXAUTH");

            migrationBuilder.DropTable(
                name: "GLBOOKS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSTAXAUTH",
                table: "CSTAXAUTH");

            migrationBuilder.AlterColumn<string>(
                name: "CMPID",
                table: "CSTAXAUTH",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TAXAUTH",
                table: "CSTAXAUTH",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "CSTAXAUTH",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CSTAXAUTH",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSTAXAUTH",
                table: "CSTAXAUTH",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXAUTH_CompanyId",
                table: "CSTAXAUTH",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CSTAXAUTH_CSSETUP_CompanyId",
                table: "CSTAXAUTH",
                column: "CompanyId",
                principalTable: "CSSETUP",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
