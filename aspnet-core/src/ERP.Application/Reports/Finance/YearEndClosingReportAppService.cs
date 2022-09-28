using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Reports.Finance
{
    public class YearEndClosingReportAppService : ERPAppServiceBase
    {
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<GLOption> _glOptionRepository;
        private readonly IRepository<FiscalCalender> _fiscalCalenderRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GroupCategory> _groupCategoryRepository;
        private readonly IRepository<ControlDetail> _controlDetailRepository;


        private VoucherEntryAppService _voucherEntryAppService;

        public YearEndClosingReportAppService(IRepository<GLTRHeader> gltrHeaderRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<ControlDetail> controlDetailRepository,IRepository<GLTRDetail> gltrDetailRepository, IRepository<GLOption> glOptionRepository, IRepository<FiscalCalender> fiscalCalenderRepository, IRepository<User, long> userRepository, VoucherEntryAppService voucherEntryAppService, IRepository<GroupCategory> groupCategoryRepository)
        {
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _glOptionRepository = glOptionRepository;
            _fiscalCalenderRepository = fiscalCalenderRepository;
            _userRepository = userRepository;
            _voucherEntryAppService = voucherEntryAppService;
            _chartofControlRepository = chartofControlRepository;
            _controlDetailRepository = controlDetailRepository;
            _groupCategoryRepository = groupCategoryRepository;
        }

        public async Task<string> CloseFiscalYearEnd()
        {
            /*=======================================
            * Check Active Year Locked
            * Create JV Auto Entry in GLTRH and GLTRD.
            * Auto BookId='CL'
            * Auto Narration='Year End Closing Entry For {Year Period}'
            * Auto GLTRH=>DocNo,Month,Date,GLSetup->(BaseCurrency,Rate)
            * Auto GLTRD=>Debit->(All the account in Fiscal Year in GLTRD Table),Credit->(Sum of amount against GLSETUP account)
            ================================= Azeem */
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var fiscalYear = _fiscalCalenderRepository.GetAll().FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.IsActive == true).Period;
            var fiscalEndDate = _fiscalCalenderRepository.GetAll().FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.IsActive == true).EndDate;
            var isLocked = _fiscalCalenderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.IsActive == true && o.IsLocked == false).Count();
            if (isLocked != 0)
            {
                alertMsg = "Active Year Not Close";
            }
            else
            {
                string bookId = "CL";

                string[] grlist = { "EXPENSE", "REVENUE" };
                string narration = "Year End Closing Entry For " + fiscalYear + "-" + (Convert.ToInt32(fiscalYear) + 1);
                var glHeader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocDate.Year == fiscalYear);
                var glDetails = _gltrDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                var groupCat = _groupCategoryRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && grlist.Contains(o.GRPCTDESC));
                var chartOfAcc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                var segment1 = _controlDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

                var accountDetailsList = (from p in glDetails
                                          group p by new { p.AccountID, p.DetID } into gd
                                          // join *after* group
                                          join bp in glHeader on gd.FirstOrDefault().DetID equals bp.Id
                                          select new CreateOrEditGLTRDetailDto
                                          {
                                              Amount = gd.Sum(x => x.Amount),
                                              AccountID = gd.Select(x => x.AccountID).FirstOrDefault(),
                                              Narration = narration,
                                              SubAccID = 0
                                          }).Where(x => x.Amount != 0).ToList();
                
                var resultDetails = (from d in accountDetailsList
                                     join ca in chartOfAcc on d.AccountID equals ca.Id
                                     join sg in segment1 on ca.ControlDetailId equals sg.Seg1ID
                                     join gc in groupCat on sg.Family equals gc.GRPCTCODE
                                     select new CreateOrEditGLTRDetailDto
                                     {
                                         Amount = d.Amount,
                                         AccountID = d.AccountID,
                                         Narration = d.Narration,
                                         SubAccID = 0
                                     }).ToList();


                VoucherEntryDto autoEntry = new VoucherEntryDto()
                {
                    GLTRHeader = new CreateOrEditGLTRHeaderDto()
                    {
                        BookID = bookId,
                        NARRATION = narration,
                        DocDate = DateTime.Now,
                        DocMonth = DateTime.Now.Month,
                        AuditUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                        AuditTime = DateTime.Now,
                        CURID = currency.Id,
                        CURRATE = currency.CurrRate
                    },
                    GLTRDetail = resultDetails
                };

                if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader.DocDate<= fiscalEndDate)
                {
                    await _voucherEntryAppService.CreateOrEditVoucherEntry(autoEntry);
                    alertMsg = "Save";
                }
                else
                {
                    alertMsg="NoRecord";
                }
            }
            return alertMsg;
        }
    }
}
