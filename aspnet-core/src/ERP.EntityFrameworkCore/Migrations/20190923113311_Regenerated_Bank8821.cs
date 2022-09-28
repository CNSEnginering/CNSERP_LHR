using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Regenerated_Bank8821 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BKBANKS_GLAMF_ChartofControlId",
                table: "BKBANKS");

            migrationBuilder.RenameColumn(
                name: "ChartofControlId",
                table: "BKBANKS",
                newName: "AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_BKBANKS_ChartofControlId",
                table: "BKBANKS",
                newName: "IX_BKBANKS_AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_BKBANKS_GLAMF_AccountID",
                table: "BKBANKS",
                column: "AccountID",
                principalTable: "GLAMF",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BKBANKS_GLAMF_AccountID",
                table: "BKBANKS");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "BKBANKS",
                newName: "ChartofControlId");

            migrationBuilder.RenameIndex(
                name: "IX_BKBANKS_AccountID",
                table: "BKBANKS",
                newName: "IX_BKBANKS_ChartofControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_BKBANKS_GLAMF_ChartofControlId",
                table: "BKBANKS",
                column: "ChartofControlId",
                principalTable: "GLAMF",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
