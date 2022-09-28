using System.Collections.Generic;
using ERP.Authorization.Users.Importing.Dto;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ControlDetail.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ControlDetail
{
    public interface IControlDetailListExcelDataReader : ITransientDependency
    {
        List<ImportControlDetailDto> GetControlDetailFromExcel(byte[] fileBytes);
    }
}
