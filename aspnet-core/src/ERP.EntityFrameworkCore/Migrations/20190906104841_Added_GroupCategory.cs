using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_GroupCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CSFSC",
                newName: "FSCYEAR");

            migrationBuilder.CreateTable(
                name: "GLGRPCAT",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    GRPCTDESC = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLGRPCAT", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLGRPCAT_TenantId",
                table: "GLGRPCAT",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GLGRPCAT");

            migrationBuilder.RenameColumn(
                name: "FSCYEAR",
                table: "CSFSC",
                newName: "Id");
        }
    }
}
