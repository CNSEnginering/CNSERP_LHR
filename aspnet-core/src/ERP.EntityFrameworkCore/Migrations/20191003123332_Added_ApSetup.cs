using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Added_ApSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BkTransfer_BKBANKS_BankId",
                table: "BkTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_CSCURRATE_CSSETUP_CMPID",
                table: "CSCURRATE");

            migrationBuilder.DropIndex(
                name: "IX_CSCURRATE_CMPID",
                table: "CSCURRATE");

            migrationBuilder.DropIndex(
                name: "IX_BkTransfer_BankId",
                table: "BkTransfer");

            migrationBuilder.DropColumn(
                name: "AppDate",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "DbID",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "DirectPost",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "FiscalEndDate",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "FiscalStartDate",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "LocDate",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "UseLocID",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "CMPID",
                table: "CSCURRATE");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "BkTransfer");

            migrationBuilder.RenameColumn(
                name: "AppSerial",
                table: "CSSETUP",
                newName: "LegalName");

            migrationBuilder.RenameColumn(
                name: "Alertmsg",
                table: "CSSETUP",
                newName: "DESIGNATION1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocDate",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Debit",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "Credit",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "DocNo",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "BookID",
                table: "V_GLTRV",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seg1Name",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seg2Name",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seg3Name",
                table: "GLSETUP",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SLType",
                table: "GLAMFSL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CONTEMAIL1",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CONTPERSON1",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CONTPHONE1",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DESIGNATION",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SYMBOL",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CURNAME",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AUDTUSER",
                table: "CSCURRATE",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BkTransfer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "APSETUP",
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
                    table.PrimaryKey("PK_APSETUP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CSFISCALLOCK",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    GL = table.Column<bool>(nullable: false),
                    AP = table.Column<bool>(nullable: false),
                    AR = table.Column<bool>(nullable: false),
                    IN = table.Column<bool>(nullable: false),
                    PO = table.Column<bool>(nullable: false),
                    OE = table.Column<bool>(nullable: false),
                    BK = table.Column<bool>(nullable: false),
                    HR = table.Column<bool>(nullable: false),
                    PR = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: true),
                    EditUser = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSFISCALLOCK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CSTAXAUTH",
                columns: table => new
                {
                    TAXAUTH = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    TAXAUTHDESC = table.Column<string>(nullable: false),
                    ACCLIABILITY = table.Column<string>(nullable: true),
                    ACCRECOVERABLE = table.Column<string>(nullable: true),
                    RECOVERRATE = table.Column<double>(nullable: false),
                    ACCEXPENSE = table.Column<string>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: false),
                    AUDTUSER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSTAXAUTH", x => x.TAXAUTH);
                });

            migrationBuilder.CreateTable(
                name: "CSTAXCLASS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    CLASSID = table.Column<int>(nullable: false),
                    TAXAUTH = table.Column<string>(nullable: true),
                    CLASSDESC = table.Column<string>(nullable: true),
                    CLASSRATE = table.Column<double>(nullable: true),
                    TRANSTYPE = table.Column<double>(nullable: true),
                    CLASSTYPE = table.Column<string>(nullable: true),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSTAXCLASS", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "GLCONFIG",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    AccountID = table.Column<string>(nullable: false),
                    SubAccID = table.Column<int>(nullable: false),
                    ConfigID = table.Column<int>(nullable: false),
                    BookID = table.Column<string>(nullable: false),
                    PostingOn = table.Column<bool>(nullable: false),
                    AUDTDATE = table.Column<DateTime>(nullable: true),
                    AUDTUSER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLCONFIG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GLTRD",
                columns: table => new
                {
                    DetailID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DetID = table.Column<int>(nullable: false),
                    AccountID = table.Column<string>(nullable: false),
                    SubAccID = table.Column<int>(nullable: true),
                    Narration = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: true),
                    ChequeNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLTRD", x => x.DetailID);
                });

            migrationBuilder.CreateTable(
                name: "GLTRH",
                columns: table => new
                {
                    DetID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    BookID = table.Column<string>(nullable: false),
                    ConfigID = table.Column<int>(nullable: false),
                    DocNo = table.Column<int>(nullable: false),
                    DocMonth = table.Column<int>(nullable: false),
                    DocDate = table.Column<DateTime>(nullable: false),
                    NARRATION = table.Column<string>(nullable: true),
                    Posted = table.Column<bool>(nullable: false),
                    AuditUser = table.Column<string>(nullable: true),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    OldCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLTRH", x => x.DetID);
                });

            migrationBuilder.CreateTable(
                name: "GLTRV",
                columns: table => new
                {
                    DetID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookID = table.Column<string>(nullable: false),
                    ConfigID = table.Column<int>(nullable: false),
                    DocNo = table.Column<int>(nullable: false),
                    DocMonth = table.Column<int>(nullable: false),
                    DocDate = table.Column<DateTime>(nullable: false),
                    AuditUser = table.Column<string>(nullable: true),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    Posted = table.Column<bool>(nullable: false),
                    BookName = table.Column<string>(nullable: true),
                    AccountID = table.Column<string>(nullable: true),
                    SubAccID = table.Column<int>(nullable: true),
                    Narration = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    SubAccName = table.Column<string>(nullable: true),
                    DetailID = table.Column<int>(nullable: true),
                    ChequeNo = table.Column<string>(nullable: false),
                    RegNo = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLTRV", x => x.DetID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APSETUP_TenantId",
                table: "APSETUP",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CSFISCALLOCK_TenantId",
                table: "CSFISCALLOCK",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXAUTH_TenantId",
                table: "CSTAXAUTH",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CSTAXCLASS_TenantId",
                table: "CSTAXCLASS",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLBOOKS_TenantId",
                table: "GLBOOKS",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLCONFIG_TenantId",
                table: "GLCONFIG",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLTRD_TenantId",
                table: "GLTRD",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GLTRH_TenantId",
                table: "GLTRH",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAMFSL_GLAMF_AccountID",
                table: "GLAMFSL",
                column: "AccountID",
                principalTable: "GLAMF",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAMFSL_GLAMF_AccountID",
                table: "GLAMFSL");

            migrationBuilder.DropTable(
                name: "APSETUP");

            migrationBuilder.DropTable(
                name: "CSFISCALLOCK");

            migrationBuilder.DropTable(
                name: "CSTAXAUTH");

            migrationBuilder.DropTable(
                name: "CSTAXCLASS");

            migrationBuilder.DropTable(
                name: "GLBOOKS");

            migrationBuilder.DropTable(
                name: "GLCONFIG");

            migrationBuilder.DropTable(
                name: "GLTRD");

            migrationBuilder.DropTable(
                name: "GLTRH");

            migrationBuilder.DropTable(
                name: "GLTRV");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "V_GLTRV");

            migrationBuilder.DropColumn(
                name: "Seg1Name",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "Seg2Name",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "Seg3Name",
                table: "GLSETUP");

            migrationBuilder.DropColumn(
                name: "SLType",
                table: "GLAMFSL");

            migrationBuilder.DropColumn(
                name: "CONTEMAIL1",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "CONTPERSON1",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "CONTPHONE1",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CSSETUP");

            migrationBuilder.DropColumn(
                name: "DESIGNATION",
                table: "CSSETUP");

            migrationBuilder.RenameColumn(
                name: "LegalName",
                table: "CSSETUP",
                newName: "AppSerial");

            migrationBuilder.RenameColumn(
                name: "DESIGNATION1",
                table: "CSSETUP",
                newName: "Alertmsg");

            migrationBuilder.AlterColumn<string>(
                name: "DocDate",
                table: "V_GLTRV",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<decimal>(
                name: "Debit",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "DocNo",
                table: "V_GLTRV",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppDate",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DbID",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DirectPost",
                table: "CSSETUP",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FiscalEndDate",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FiscalStartDate",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LocDate",
                table: "CSSETUP",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseLocID",
                table: "CSSETUP",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "SYMBOL",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CURNAME",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AUDTUSER",
                table: "CSCURRATE",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CMPID",
                table: "CSCURRATE",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BkTransfer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "BkTransfer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CSCURRATE_CMPID",
                table: "CSCURRATE",
                column: "CMPID");

            migrationBuilder.CreateIndex(
                name: "IX_BkTransfer_BankId",
                table: "BkTransfer",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BkTransfer_BKBANKS_BankId",
                table: "BkTransfer",
                column: "BankId",
                principalTable: "BKBANKS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CSCURRATE_CSSETUP_CMPID",
                table: "CSCURRATE",
                column: "CMPID",
                principalTable: "CSSETUP",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
