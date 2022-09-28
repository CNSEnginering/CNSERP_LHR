using System.Collections.Generic;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SubControlDetail
{
    public interface ISubControlDetailListExcelDataReader : ITransientDependency
    {
        List<ImportSubControlDetailDto> GetSubControlDetailFromExcel(byte[] fileBytes);
    }
}
