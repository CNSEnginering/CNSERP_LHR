using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_GLLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAMF_GLSEG1_Segment1",
                table: "GLAMF");

            migrationBuilder.DropForeignKey(
                name: "FK_GLAMF_GLSEG3_Segment3",
                table: "GLAMF");

            migrationBuilder.DropForeignKey(
                name: "FK_GLAMF_GLSEG2_Segment2",
                table: "GLAMF");

            migrationBuilder.DropForeignKey(
                name: "FK_GLAMFSL_GLAMF_AccountID",
                table: "GLAMFSL");

            migrationBuilder.DropForeignKey(
                name: "FK_GLGRPCODE_GLGRPCAT_GRPCTCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropForeignKey(
                name: "FK_GLSEG1_GLGRPCAT_Family",
                table: "GLSEG1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG3",
                table: "GLSEG3");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG2",
                table: "GLSEG2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1");

            migrationBuilder.DropIndex(
                name: "IX_GLSEG1_Family",
                table: "GLSEG1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropIndex(
                name: "IX_GLGRPCODE_GRPCTCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLGRPCAT",
                table: "GLGRPCAT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL");

            migrationBuilder.DropIndex(
                name: "IX_GLAMFSL_TenantId",
                table: "GLAMFSL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAMF",
                table: "GLAMF");

            migrationBuilder.DropIndex(
                name: "IX_GLAMF_Segment1",
                table: "GLAMF");

            migrationBuilder.DropIndex(
                name: "IX_GLAMF_Segment3",
                table: "GLAMF");

            migrationBuilder.DropIndex(
                name: "IX_GLAMF_Segment2",
                table: "GLAMF");

            migrationBuilder.DropIndex(
                name: "IX_GLAMF_TenantId",
                table: "GLAMF");

            migrationBuilder.DropIndex(
                name: "IX_CSTAXAUTH_TenantId",
                table: "CSTAXAUTH");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE");

            migrationBuilder.DropIndex(
                name: "IX_CSCURRATE_TenantId",
                table: "CSCURRATE");

            migrationBuilder.DropColumn(
                name: "ALLOWPOSTPREV",
                table: "GLSETUP");

            migrationBuilder.RenameColumn(
                name: "ALLOWPOSTPROV",
                table: "GLSETUP",
                newName: "DirectPost");

            migrationBuilder.AddColumn<string>(
                name: "CURID",
                table: "GLTRH",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CURRATE",
                table: "GLTRH",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAuto",
                table: "GLTRD",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSETUP",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GLSETUP",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "FifthSignature",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstSignature",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FourthSignature",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondSignature",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SixthSignature",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdSignature",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSEG3",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Seg3ID",
                table: "GLSEG3",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GLSEG3",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSEG2",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Seg2ID",
                table: "GLSEG2",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GLSEG2",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Seg1ID",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GLSEG1",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLGRPCODE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLGRPCAT",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GRPCTCODE",
                table: "GLGRPCAT",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GLGRPCAT",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLCONFIG",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLBOOKS",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLAMFSL",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityID",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceID",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLAMF",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Segment2",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Segment3",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Segment1",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupCode",
                table: "GLAMF",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CSTAXCLASS",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "CSSETUP",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign1",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign2",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign3",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign4",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign5",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "CSFISCALLOCK",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CSFISCALLOCK",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "CSFISCALLOCK",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Decimal",
                table: "CSCURRATE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BKBANKS",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TermType",
                table: "APTERMS",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "DEFCURRCODE",
                table: "APSETUP",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DEFBANKID",
                table: "APSETUP",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PAYTERMS",
                table: "APSETUP",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG3",
                table: "GLSEG3",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG2",
                table: "GLSEG2",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLGRPCAT",
                table: "GLGRPCAT",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL",
                columns: new[] { "AccountID", "SubAccID", "TenantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAMF",
                table: "GLAMF",
                columns: new[] { "AccountID", "TenantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE",
                columns: new[] { "CURID", "TenantId" });

            migrationBuilder.CreateTable(
                name: "APSETUP1",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DEFBANKID = table.Column<int>(nullable: true),
                    DEFPAYCODE = table.Column<int>(nullable: true),
                    DEFVENCTRLACC = table.Column<string>(nullable: true),
                    DEFCURRCODE = table.Column<int>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APSETUP1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ARSETUP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DEFBANKID = table.Column<string>(nullable: true),
                    DEFPAYCODE = table.Column<int>(nullable: true),
                    DEFCUSCTRLACC = table.Column<string>(nullable: true),
                    DEFCURRCODE = table.Column<string>(nullable: true),
                    PAYTERMS = table.Column<string>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARSETUP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GLLOC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    LocDesc = table.Column<string>(nullable: true),
                    AuditUser = table.Column<string>(nullable: true),
                    AuditDate = table.Column<DateTime>(nullable: true),
                    LocId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLLOC", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLAMFSL_AccountID_TenantId",
                table: "GLAMFSL",
                columns: new[] { "AccountID", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXAUTH_TenantId_TAXAUTH",
                table: "CSTAXAUTH",
                columns: new[] { "TenantId", "TAXAUTH" });

            migrationBuilder.CreateIndex(
                name: "IX_APSETUP1_TenantId",
                table: "APSETUP1",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ARSETUP_TenantId",
                table: "ARSETUP",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLLOC_TenantId",
                table: "GLLOC",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMFSL_GLAMF_AccountID_TenantId",
                table: "GLAMFSL",
                columns: new[] { "AccountID", "TenantId" },
                principalTable: "GLAMF",
                principalColumns: new[] { "AccountID", "TenantId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAMFSL_GLAMF_AccountID_TenantId",
                table: "GLAMFSL");

            migrationBuilder.DropTable(
                name: "APSETUP1");

            migrationBuilder.DropTable(
                name: "ARSETUP");

            migrationBuilder.DropTable(
                name: "GLLOC");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG3",
                table: "GLSEG3");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG2",
                table: "GLSEG2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLGRPCAT",
                table: "GLGRPCAT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL");

            migrationBuilder.DropIndex(
                name: "IX_GLAMFSL_AccountID_TenantId",
                table: "GLAMFSL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GLAMF",
                table: "GLAMF");

            migrationBuilder.DropIndex(
                name: "IX_CSTAXAUTH_TenantId_TAXAUTH",
                table: "CSTAXAUTH");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE");

            migrationBuilder.DropColumn(
                name: "CURID",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "CURRATE",
                table: "GLTRH");

            migrationBuilder.DropColumn(
                name: "IsAuto",
                table: "GLTRD");

            migrationBuilder.DropColumn(
                name: "FifthSignature",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "FirstSignature",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "FourthSignature",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "SecondSignature",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "SixthSignature",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "ThirdSignature",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLSEG3");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLSEG2");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLSEG1");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLGRPCODE");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GLGRPCAT");

            migrationBuilder.DropColumn(
                name: "CityID",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "ProvinceID",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                table: "GLAMF");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CSTAXCLASS");

            migrationBuilder.DropColumn(
                name: "Sign1",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "Sign2",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "Sign3",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "Sign4",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "Sign5",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CSFISCALLOCK");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "CSFISCALLOCK");

            migrationBuilder.DropColumn(
                name: "Decimal",
                table: "CSCURRATE");

            migrationBuilder.DropColumn(
                name: "TermType",
                table: "APTERMS");

            migrationBuilder.DropColumn(
                name: "PAYTERMS",
                table: "APSETUP");

            migrationBuilder.RenameColumn(
                name: "DirectPost",
                table: "GLSETUP",
                newName: "ALLOWPOSTPROV");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSETUP",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "GLSETUP",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "ALLOWPOSTPREV",
                table: "GLSETUP",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSEG3",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Seg3ID",
                table: "GLSEG3",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSEG2",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Seg2ID",
                table: "GLSEG2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLSEG1",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Seg1ID",
                table: "GLSEG1",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLGRPCODE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GRPCODE",
                table: "GLGRPCODE",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLGRPCAT",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GRPCTCODE",
                table: "GLGRPCAT",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLCONFIG",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLBOOKS",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLAMFSL",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Segment2",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Segment3",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Segment1",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "GLAMF",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "CSSETUP",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "CSFISCALLOCK",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BKBANKS",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DEFCURRCODE",
                table: "APSETUP",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DEFBANKID",
                table: "APSETUP",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG3",
                table: "GLSEG3",
                column: "Seg3ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG2",
                table: "GLSEG2",
                column: "Seg2ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLSEG1",
                table: "GLSEG1",
                column: "Seg1ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLGRPCODE",
                table: "GLGRPCODE",
                column: "GRPCODE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLGRPCAT",
                table: "GLGRPCAT",
                column: "GRPCTCODE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAMFSL",
                table: "GLAMFSL",
                columns: new[] { "AccountID", "SubAccID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GLAMF",
                table: "GLAMF",
                column: "AccountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CSCURRATE",
                table: "CSCURRATE",
                column: "CURID");

            migrationBuilder.CreateIndex(
                name: "IX_GLSEG1_Family",
                table: "GLSEG1",
                column: "Family");

            migrationBuilder.CreateIndex(
                name: "IX_GLGRPCODE_GRPCTCODE",
                table: "GLGRPCODE",
                column: "GRPCTCODE");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMFSL_TenantId",
                table: "GLAMFSL",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMF_Segment1",
                table: "GLAMF",
                column: "Segment1");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMF_Segment3",
                table: "GLAMF",
                column: "Segment3");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMF_Segment2",
                table: "GLAMF",
                column: "Segment2");

            migrationBuilder.CreateIndex(
                name: "IX_GLAMF_TenantId",
                table: "GLAMF",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXAUTH_TenantId",
                table: "CSTAXAUTH",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CSCURRATE_TenantId",
                table: "CSCURRATE",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMF_GLSEG1_Segment1",
                table: "GLAMF",
                column: "Segment1",
                principalTable: "GLSEG1",
                principalColumn: "Seg1ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMF_GLSEG3_Segment3",
                table: "GLAMF",
                column: "Segment3",
                principalTable: "GLSEG3",
                principalColumn: "Seg3ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMF_GLSEG2_Segment2",
                table: "GLAMF",
                column: "Segment2",
                principalTable: "GLSEG2",
                principalColumn: "Seg2ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMFSL_GLAMF_AccountID",
                table: "GLAMFSL",
                column: "AccountID",
                principalTable: "GLAMF",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GLGRPCODE_GLGRPCAT_GRPCTCODE",
                table: "GLGRPCODE",
                column: "GRPCTCODE",
                principalTable: "GLGRPCAT",
                principalColumn: "GRPCTCODE",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GLSEG1_GLGRPCAT_Family",
                table: "GLSEG1",
                column: "Family",
                principalTable: "GLGRPCAT",
                principalColumn: "GRPCTCODE",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
