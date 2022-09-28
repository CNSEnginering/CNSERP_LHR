using Abp.Domain.Repositories;
using ERP.PayRoll.Grades;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class GradeListingAppService : ERPReportAppServiceBase, IGradeListingAppService
    {
        private readonly IRepository<Grade> _gradeRepository;

        public GradeListingAppService(IRepository<Grade> gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }
        public List<GradeListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _gradeRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.GradeID >= Convert.ToInt32(fromCode) && o.GradeID <= Convert.ToInt32(toCode) &&
            o.GradeName.Contains(description))
                       select new GradeListingDto()
                       {
                           GradeID = a.GradeID.ToString(),
                           GradeName = a.GradeName,
                           Type = a.Type.ToString(),
                       };

            return data.ToList();
        }
    }
}
