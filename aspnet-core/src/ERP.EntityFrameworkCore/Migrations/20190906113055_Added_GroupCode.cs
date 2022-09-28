using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_GroupCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GLGRPCAT",
                newName: "GRPCTCODE");

            migrationBuilder.CreateTable(
                name: "GLGRPCODE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    GRPCODE = table.Column<int>(nullable: false),
                    GRPDESC = table.Column<string>(nullable: true),
                    GRPCTCODE = table.Column<string>(nullable: true),
                    GroupCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLGRPCODE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GLGRPCODE_GLGRPCAT_GroupCategoryId",
                        column: x => x.GroupCategoryId,
                        principalTable: "GLGRPCAT",
                        principalColumn: "GRPCTCODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLGRPCODE_GroupCategoryId",
                table: "GLGRPCODE",
                column: "GroupCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GLGRPCODE_TenantId",
                table: "GLGRPCODE",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GLGRPCODE");

            migrationBuilder.RenameColumn(
                name: "GRPCTCODE",
                table: "GLGRPCAT",
                newName: "Id");
        }
    }
}
