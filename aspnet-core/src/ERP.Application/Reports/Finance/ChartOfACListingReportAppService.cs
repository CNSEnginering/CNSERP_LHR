using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.CommonServices;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.SetupForms.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports.Finance
{
    public class ChartOfACListingReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<CharOfACListing> _chartofACListingRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<GroupCategory> _groupCategoryRepository;
        private readonly IRepository<ControlDetail> _seg1Repository;
        private readonly IRepository<SubControlDetail> _seg2Repository;
        private readonly IRepository<Segmentlevel3> _seg3Repository;
        private readonly IRepository<GroupCode> _groupCodeRepository;
        private readonly IRepository<CompanyProfile, string> _companyProfileRepository;


        public ChartOfACListingReportAppService(IRepository<CompanyProfile, string> companyProfileRepository, IRepository<SubControlDetail> seg2Repository,
            IRepository<GroupCode> groupCodeRepository, IRepository<Segmentlevel3> seg3Repository, IRepository<ChartofControl, string>
            chartofControlRepository, IRepository<GroupCategory> groupCategoryRepository, IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<ControlDetail> controlDetailRepository, IRepository<CharOfACListing> chartofACListingRepository)
        {
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _seg1Repository = controlDetailRepository;
            _groupCategoryRepository = groupCategoryRepository;
            _seg2Repository = seg2Repository;
            _seg3Repository = seg3Repository;
            _groupCodeRepository = groupCodeRepository;
            _companyProfileRepository = companyProfileRepository;
            _chartofACListingRepository = chartofACListingRepository;
        }

        public List<ChartOfAccountListDto> GetChartOfAccountsData(int slType, string fromAccount, string toAccount)
        {
            var tenantId = AbpSession.TenantId;
            //var groupCat = _groupCategoryRepository.GetAll().Where(o => o.TenantId == tenantId);
            //var chartOfAcc1 = _chartofControlRepository.GetAll().Where(o => o.TenantId == tenantId).Where(d => d.Id.CompareTo(fromAccount) >= 0 && d.Id.CompareTo(toAccount) <= 0).WhereIf(slType != 0, e => e.SLType == slType );
            var chartOfAcc = _chartofACListingRepository.GetAll().Where(o => o.TenantId == tenantId).Where(d => d.AccountID.CompareTo(fromAccount) >= 0 && d.AccountID.CompareTo(toAccount) <= 0).WhereIf(slType != 0, e => e.SLType == slType);
            //var segment1 = _seg1Repository.GetAll().Where(o => o.TenantId == tenantId);
            //var segment2 = _seg2Repository.GetAll().Where(o => o.TenantId == tenantId);
            //var segment3 = _seg3Repository.GetAll().Where(o => o.TenantId == tenantId);
            //var grpCode = _groupCodeRepository.GetAll().Where(o => o.TenantId == tenantId);
            var company = _companyProfileRepository.GetAll().Where(e => e.TenantId == tenantId);


            //var chartOfAccountList = (from acc in chartOfAcc1
            //                          join seg1 in segment1 on acc.ControlDetailId equals seg1.Seg1ID
            //                          join seg2 in segment2 on acc.SubControlDetailId equals seg2.Seg2ID
            //                          join seg3 in segment3 on acc.Segmentlevel3Id equals seg3.Seg3ID
            //                          join gcat in groupCat on seg1.Family equals gcat.GRPCTCODE into ct
            //                          from gct in ct.DefaultIfEmpty()
            //                          join gcode in grpCode on gct.GRPCTCODE equals gcode.GRPCTCODE into cd
            //                          from gcod in cd.DefaultIfEmpty()
            //                          group acc by new
            //                          {
            //                              acc.Id,
            //                              acc.AccountName,
            //                              acc.SubLedger,
            //                              acc.ControlDetailId,
            //                              acc.SubControlDetailId,
            //                              acc.Segmentlevel3Id,
            //                              gct.GRPCTDESC,
            //                              gcod.GRPDESC,
            //                              seg1.SegmentName,
            //                              Segment2Name = seg2.SegmentName,
            //                              Segment3Name = seg3.SegmentName
            //                          } into g
            //                          select new ChartOfAccountListDto
            //                          {
            //                              AccountCode = g.Key.Id,
            //                              AccountTitle = g.Key.AccountName,
            //                              Family = g.Key.GRPCTDESC,
            //                              SubLedger = g.Key.SubLedger,
            //                              GroupDesc = g.Key.GRPDESC,
            //                              Segment1 = g.Key.ControlDetailId,
            //                              Segment1Name = g.Key.SegmentName,
            //                              Segment2 = g.Key.SubControlDetailId,
            //                              Segment2Name = g.Key.Segment2Name,
            //                              Segment3 = g.Key.Segmentlevel3Id,
            //                              Segment3Name = g.Key.Segment2Name,
            //                              CompanyName = company.SingleOrDefault().CompanyName,
            //                              CompanyAddress = company.SingleOrDefault().Address1,
            //                              CompanyPhone = company.SingleOrDefault().Phone
            //                          }).ToList();
            var chartOfAccountList = (from acc in chartOfAcc
                                      orderby acc.AccountID
                                      select new ChartOfAccountListDto()
                                      {
                                          AccountCode = acc.AccountID,
                                          AccountTitle = acc.AccountName,
                                          Family = acc.GRPCTDESC,
                                          SubLedger = acc.SubLedger,
                                          GroupDesc = acc.GRPDESC,
                                          Segment1 = acc.Seg1ID,
                                          Segment1Name = acc.Seg1Name,
                                          Segment2 = acc.Seg2ID,
                                          Segment2Name = acc.Seg2Name,
                                          Segment3 = acc.Seg3ID,
                                          Segment3Name = acc.Seg3Name,
                                          CompanyName = company.SingleOrDefault().CompanyName,
                                          CompanyAddress = company.SingleOrDefault().Address1,
                                          CompanyPhone = company.SingleOrDefault().Phone

                                      }).ToList();

            string pjson = JsonConvert.SerializeObject(chartOfAccountList);
            return chartOfAccountList.ToList();
        }

        public List<SubledgerListDto> GetSubLedgerData(int slType)
        {
            var tenantId = AbpSession.TenantId;
            var chartOfAcc = _chartofControlRepository.GetAll().Where(o => o.TenantId == tenantId).WhereIf(slType != 0, e => e.SLType == slType);
            var subledger = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == tenantId);
            var company = _companyProfileRepository.GetAll().Where(e => e.TenantId == tenantId);
            var subledgerList = from acc in chartOfAcc
                                join sl in subledger on acc.Id equals sl.AccountID
                                select new SubledgerListDto
                                {
                                    AccountCode = acc.Id,
                                    AccountTitle = acc.AccountName,
                                    SubAccID = sl.Id,
                                    SubAccTitle = sl.SubAccName,
                                    Address = sl.Address1,
                                    City = sl.City,
                                    Phone = sl.Phone,
                                    CompanyName = company.SingleOrDefault().CompanyName,
                                    CompanyAddress = company.SingleOrDefault().Address1,
                                    CompanyPhone = company.SingleOrDefault().Phone
                                };

            string pjson = JsonConvert.SerializeObject(subledgerList);
            return subledgerList.ToList();
        }
    }

    public class ChartOfAccountListDto
    {
        public string AccountCode { get; set; }

        public string AccountTitle { get; set; }

        public string Family { get; set; }

        public bool SubLedger { get; set; }

        public string GroupDesc { get; set; }

        public string Segment1 { get; set; }

        public string Segment1Name { get; set; }

        public string Segment2 { get; set; }

        public string Segment2Name { get; set; }

        public string Segment3 { get; set; }

        public string Segment3Name { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyPhone { get; set; }
    }

    public class SubledgerListDto
    {
        public string AccountCode { get; set; }

        public string AccountTitle { get; set; }

        public int SubAccID { get; set; }

        public string SubAccTitle { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyPhone { get; set; }
    }
}
