using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_Segmentlevel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLSEG1");

            migrationBuilder.AlterColumn<string>(
                name: "Seg1ID",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Family",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1",
                column: "Seg1ID");

            migrationBuilder.CreateTable(
                name: "GLSEG2",
                columns: table => new
                {
                    Seg2ID = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    SegmentName = table.Column<string>(nullable: true),
                    OldCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLSEG2", x => x.Seg2ID);
                });

            migrationBuilder.CreateTable(
                name: "GLSEG3",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Seg3ID = table.Column<string>(nullable: false),
                    SegmentName = table.Column<string>(nullable: true),
                    OldCode = table.Column<string>(nullable: true),
                    ControlDetailId = table.Column<string>(nullable: true),
                    SubControlDetailId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLSEG3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GLSEG3_GLSEG1_ControlDetailId",
                        column: x => x.ControlDetailId,
                        principalTable: "GLSEG1",
                        principalColumn: "Seg1ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GLSEG3_GLSEG2_SubControlDetailId",
                        column: x => x.SubControlDetailId,
                        principalTable: "GLSEG2",
                        principalColumn: "Seg2ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG1_Family",
                table: "GLSEG1",
                column: "Family");

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG2_TenantId",
                table: "GLSEG2",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG3_ControlDetailId",
                table: "GLSEG3",
                column: "ControlDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG3_SubControlDetailId",
                table: "GLSEG3",
                column: "SubControlDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG3_TenantId",
                table: "GLSEG3",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLSEG1_GLGRPCAT_Family",
                table: "GLSEG1",
                column: "Family",
                principalTable: "GLGRPCAT",
                principalColumn: "GRPCTCODE",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLSEG1_GLGRPCAT_Family",
                table: "GLSEG1");

            migrationBuilder.DropTable(
                name: "GLSEG3");

            migrationBuilder.DropTable(
                name: "GLSEG2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1");

            migrationBuilder.DropIndex(
                name: "IX_GLSEG1_Family",
                table: "GLSEG1");

            migrationBuilder.AlterColumn<string>(
                name: "Family",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Seg1ID",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "GLSEG1",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1",
                column: "Id");
        }
    }
}
