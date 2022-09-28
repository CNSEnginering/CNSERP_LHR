using System.Collections.Generic;
using ERP.Authorization.Users.Importing.Dto;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SubLedger
{
    public interface ISubLedgerListExcelDataReader : ITransientDependency
    {
        List<ImportSubLedgerDto> GetSubledgerFromExcel(byte[] fileBytes);
    }
}
