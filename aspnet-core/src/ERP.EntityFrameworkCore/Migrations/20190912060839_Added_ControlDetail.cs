using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_ControlDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLGRPCODE_GLGRPCAT_GroupCategoryId",
                table: "GLGRPCODE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropIndex(
                name: "IX_GLGRPCODE_GroupCategoryId",
                table: "GLGRPCODE");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLGRPCODE");

            migrationBuilder.DropColumn(
                name: "GroupCategoryId",
                table: "GLGRPCODE");

            migrationBuilder.AlterColumn<int>(
                name: "GRPCTCODE",
                table: "GLGRPCODE",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GRPCODE",
                table: "GLGRPCODE",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE",
                column: "GRPCODE");

            migrationBuilder.CreateTable(
                name: "GLSEG1",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Seg1ID = table.Column<string>(nullable: false),
                    SegmentName = table.Column<string>(nullable: false),
                    Family = table.Column<string>(nullable: false),
                    OldCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLSEG1", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLGRPCODE_GRPCTCODE",
                table: "GLGRPCODE",
                column: "GRPCTCODE");

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG1_TenantId",
                table: "GLSEG1",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLGRPCODE_GLGRPCAT_GRPCTCODE",
                table: "GLGRPCODE",
                column: "GRPCTCODE",
                principalTable: "GLGRPCAT",
                principalColumn: "GRPCTCODE",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLGRPCODE_GLGRPCAT_GRPCTCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropTable(
                name: "GLSEG1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropIndex(
                name: "IX_GLGRPCODE_GRPCTCODE",
                table: "GLGRPCODE");

            migrationBuilder.AlterColumn<string>(
                name: "GRPCTCODE",
                table: "GLGRPCODE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GRPCODE",
                table: "GLGRPCODE",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GLGRPCODE",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "GroupCategoryId",
                table: "GLGRPCODE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GLGRPCODE_GroupCategoryId",
                table: "GLGRPCODE",
                column: "GroupCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLGRPCODE_GLGRPCAT_GroupCategoryId",
                table: "GLGRPCODE",
                column: "GroupCategoryId",
                principalTable: "GLGRPCAT",
                principalColumn: "GRPCTCODE",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
