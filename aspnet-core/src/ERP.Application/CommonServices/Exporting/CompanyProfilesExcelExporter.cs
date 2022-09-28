using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.Exporting
{
    public class CompanyProfilesExcelExporter : EpPlusExcelExporterBase, ICompanyProfilesExcelExporter
    {
        //test

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CompanyProfilesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCompanyProfileForViewDto> companyProfiles)
        {
            return CreateExcelPackage(
                "CompanyProfiles.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CompanyProfiles"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CompanyName"),
                        L("Phone")
                        );

                    AddObjects(
                        sheet, 2, companyProfiles,
                        _ => _.CompanyProfile.CompanyName,
                        _ => _.CompanyProfile.Phone
                        );

					

                });
        }
    }
}
