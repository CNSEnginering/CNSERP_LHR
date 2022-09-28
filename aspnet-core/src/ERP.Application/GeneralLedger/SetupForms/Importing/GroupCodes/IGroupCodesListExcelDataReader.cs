using System.Collections.Generic;
using ERP.Authorization.Users.Importing.Dto;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCodes.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCodes
{
    public interface IGroupCodesListExcelDataReader : ITransientDependency
    {
        List<ImportGroupCodesDto> GetGroupCodesFromExcel(byte[] fileBytes);
    }
}
