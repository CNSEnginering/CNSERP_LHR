using System.Collections.Generic;
using ERP.Authorization.Users.Importing.Dto;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ChartofAccount
{
    public interface IChartofAccountListExcelDataReader : ITransientDependency
    {
        List<ImportChartofAccountDto> GetChartofAccountFromExcel(byte[] fileBytes);
    }
}
