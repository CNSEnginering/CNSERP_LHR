using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_CSTAXAUTH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CSCURRATEses",
                table: "CSCURRATEses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CSCURRATEses");

            migrationBuilder.AlterColumn<string>(
                name: "CURID",
                table: "CSCURRATEses",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CMPID",
                table: "CSCURRATEses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSCURRATEses",
                table: "CSCURRATEses",
                column: "CURID");

            migrationBuilder.CreateTable(
                name: "CSTAXAUTH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    CMPID = table.Column<string>(nullable: false),
                    TAXAUTH = table.Column<string>(nullable: false),
                    TAXAUTHDESC = table.Column<string>(nullable: false),
                    ACCLIABILITY = table.Column<string>(nullable: false),
                    ACCRECOVERABLE = table.Column<string>(nullable: false),
                    RECOVERRATE = table.Column<double>(nullable: false),
                    ACCEXPENSE = table.Column<string>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: false),
                    AUDTUSER = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSTAXAUTH", x => x.Id);
                });

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
                name: "PK_CSCURRATEses",
                table: "CSCURRATEses");

            migrationBuilder.DropColumn(
                name: "CMPID",
                table: "CSCURRATEses");

            migrationBuilder.AlterColumn<string>(
                name: "CURID",
                table: "CSCURRATEses",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CSCURRATEses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSCURRATEses",
                table: "CSCURRATEses",
                column: "Id");
        }
    }
}
