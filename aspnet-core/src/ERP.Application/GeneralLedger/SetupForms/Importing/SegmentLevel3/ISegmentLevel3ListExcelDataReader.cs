using System.Collections.Generic;
using Abp.Dependency;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3
{
    public interface ISegmentLevel3ListExcelDataReader : ITransientDependency
    {
        List<ImportSegmentLevel3Dto> GetSegmentLevel3FromExcel(byte[] fileBytes);
    }
}
