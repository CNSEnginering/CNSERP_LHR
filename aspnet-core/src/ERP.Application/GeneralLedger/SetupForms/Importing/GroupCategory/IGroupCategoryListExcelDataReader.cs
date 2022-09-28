using System.Collections.Generic;
using ERP.Authorization.Users.Importing.Dto;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCategory.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCategory
{
    public interface IGroupCategoryListExcelDataReader : ITransientDependency
    {
        List<ImportGroupCategoryDto> GetGroupCategoryFromExcel(byte[] fileBytes);
    }
}
