

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Configuration;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_FiscalCalenders)]
    public class FiscalCalendersAppService : ERPAppServiceBase, IFiscalCalendersAppService
    {
        private readonly IRepository<FiscalCalender> _fiscalCalenderRepository;
        private readonly IFiscalCalendersExcelExporter _fiscalCalendersExcelExporter;


        public FiscalCalendersAppService(IRepository<FiscalCalender> fiscalCalenderRepository, IFiscalCalendersExcelExporter fiscalCalendersExcelExporter)
        {
            _fiscalCalenderRepository = fiscalCalenderRepository;
            _fiscalCalendersExcelExporter = fiscalCalendersExcelExporter;

        }

        public async Task<PagedResultDto<GetFiscalCalenderForViewDto>> GetAll(GetAllFiscalCalendersInput input)
        {
            int? tenantId = (int?)AbpSession.TenantId;
            var filteredFiscalCalenders = _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == tenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MaxPeriodFilter != null, e => e.Period == input.MaxPeriodFilter)
                        .OrderByDescending(x => x.Id).Take(12).OrderBy(x => x.Id);

            var pagedAndFilteredFiscalCalenders = filteredFiscalCalenders
                .OrderBy(x => x.Id);
            //.PageBy(input)

            var fiscalCalenders = from o in filteredFiscalCalenders
                                  select new GetFiscalCalenderForViewDto()
                                  {
                                      FiscalCalender = new FiscalCalenderDto
                                      {
                                          Period = o.Period,
                                          StartDate = o.StartDate,
                                          EndDate = o.EndDate,
                                          GL = o.GL,
                                          AP = o.AP,
                                          AR = o.AR,
                                          IN = o.IN,
                                          PO = o.PO,
                                          OE = o.OE,
                                          BK = o.BK,
                                          HR = o.HR,
                                          PR = o.PR,
                                          CreatedBy = o.CreatedBy,
                                          CreatedDate = o.CreatedDate,
                                          EditDate = o.EditDate,
                                          EditUser = o.EditUser,
                                          Id = o.Id
                                      }
                                  };

            var totalCount = await filteredFiscalCalenders.CountAsync();

            return new PagedResultDto<GetFiscalCalenderForViewDto>(
                totalCount,
                await fiscalCalenders.ToListAsync()
            );
        }

        public async Task<GetFiscalCalenderForViewDto> GetFiscalCalenderForView(int id)
        {
            var fiscalCalender = await _fiscalCalenderRepository.GetAsync(id);

            var output = new GetFiscalCalenderForViewDto { FiscalCalender = ObjectMapper.Map<FiscalCalenderDto>(fiscalCalender) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FiscalCalenders_Edit)]
        public async Task<GetFiscalCalenderForEditOutput> GetFiscalCalenderForEdit(EntityDto input)
        {
            var fiscalCalender = await _fiscalCalenderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFiscalCalenderForEditOutput { FiscalCalender = ObjectMapper.Map<CreateOrEditFiscalCalenderDto>(fiscalCalender) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFiscalCalenderDto input)
        {
            List<CreateOrEditFiscalCalenderDto> lstCalendar = new List<CreateOrEditFiscalCalenderDto>();
            if (input.Id == null)
            {
                DateTime calendarDate = new DateTime(input.Period, FiscalCalenderConsts.FiscalYearStartMonth + 1, FiscalCalenderConsts.FiscalYearStartDate);
                int currentMonth = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (i > 5)
                    {
                        calendarDate = new DateTime(input.Period + 1, calendarDate.Month, 1);
                        currentMonth = i - 5;
                    }
                    else
                        currentMonth = calendarDate.Month + i;
                    var startOfMonth = new DateTime(calendarDate.Year, currentMonth, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    lstCalendar.Add(new CreateOrEditFiscalCalenderDto
                    {
                        StartDate = startOfMonth,
                        Period = input.Period,
                        EndDate = endOfMonth,
                        CreatedBy = (int)AbpSession.UserId,
                        CreatedDate = DateTime.Now,
                        IsActive = input.IsActive

                    });
                }
                foreach (var item in lstCalendar)
                {
                    await Create(item);
                }
            }
            else
            {
                var updateFiscal = await _fiscalCalenderRepository.FirstOrDefaultAsync((int)input.Id);
                input.StartDate = updateFiscal.StartDate;
                input.EndDate = updateFiscal.EndDate;
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FiscalCalenders_Create)]
        protected virtual async Task Create(CreateOrEditFiscalCalenderDto input)
        {
            var fiscalCalender = ObjectMapper.Map<FiscalCalender>(input);


            if (AbpSession.TenantId != null)
            {
                fiscalCalender.TenantId = (int?)AbpSession.TenantId;
            }


            await _fiscalCalenderRepository.InsertAsync(fiscalCalender);
        }

        [AbpAuthorize(AppPermissions.Pages_FiscalCalenders_Edit)]
        protected virtual async Task Update(CreateOrEditFiscalCalenderDto input)
        {
            var fiscalCalender = await _fiscalCalenderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, fiscalCalender);
        }

        [AbpAuthorize(AppPermissions.Pages_FiscalCalenders_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _fiscalCalenderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFiscalCalendersToExcel(GetAllFiscalCalendersForExcelInput input)
        {

            var filteredFiscalCalenders = _fiscalCalenderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPeriodFilter != null, e => e.Period >= input.MinPeriodFilter)
                        .WhereIf(input.MaxPeriodFilter != null, e => e.Period <= input.MaxPeriodFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.GLFilter > -1, e => Convert.ToInt32(e.GL) == input.GLFilter)
                        .WhereIf(input.APFilter > -1, e => Convert.ToInt32(e.AP) == input.APFilter)
                        .WhereIf(input.ARFilter > -1, e => Convert.ToInt32(e.AR) == input.ARFilter)
                        .WhereIf(input.INFilter > -1, e => Convert.ToInt32(e.IN) == input.INFilter)
                        .WhereIf(input.POFilter > -1, e => Convert.ToInt32(e.PO) == input.POFilter)
                        .WhereIf(input.OEFilter > -1, e => Convert.ToInt32(e.OE) == input.OEFilter)
                        .WhereIf(input.BKFilter > -1, e => Convert.ToInt32(e.BK) == input.BKFilter)
                        .WhereIf(input.HRFilter > -1, e => Convert.ToInt32(e.HR) == input.HRFilter)
                        .WhereIf(input.PRFilter > -1, e => Convert.ToInt32(e.PR) == input.PRFilter)
                        .WhereIf(input.MinCreatedByFilter != null, e => e.CreatedBy >= input.MinCreatedByFilter)
                        .WhereIf(input.MaxCreatedByFilter != null, e => e.CreatedBy <= input.MaxCreatedByFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(input.MinEditDateFilter != null, e => e.EditDate >= input.MinEditDateFilter)
                        .WhereIf(input.MaxEditDateFilter != null, e => e.EditDate <= input.MaxEditDateFilter)
                        .WhereIf(input.MinEditUserFilter != null, e => e.EditUser >= input.MinEditUserFilter)
                        .WhereIf(input.MaxEditUserFilter != null, e => e.EditUser <= input.MaxEditUserFilter);

            var query = (from o in filteredFiscalCalenders
                         select new GetFiscalCalenderForViewDto()
                         {
                             FiscalCalender = new FiscalCalenderDto
                             {
                                 Period = o.Period,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 GL = o.GL,
                                 AP = o.AP,
                                 AR = o.AR,
                                 IN = o.IN,
                                 PO = o.PO,
                                 OE = o.OE,
                                 BK = o.BK,
                                 HR = o.HR,
                                 PR = o.PR,
                                 CreatedBy = o.CreatedBy,
                                 CreatedDate = o.CreatedDate,
                                 EditDate = o.EditDate,
                                 EditUser = o.EditUser,
                                 Id = o.Id
                             }
                         });


            var fiscalCalenderListDtos = await query.ToListAsync();

            return _fiscalCalendersExcelExporter.ExportToFile(fiscalCalenderListDtos);
        }

        public async Task<GetFiscalCalenderForEditOutput> GetLastYear()
        {
            int? tenantId = (int?)AbpSession.TenantId;
            var fiscalCalender = await _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == tenantId).Select(x => x).OrderByDescending(o => o.Id).FirstOrDefaultAsync();
            var output = new GetFiscalCalenderForEditOutput { FiscalCalender = ObjectMapper.Map<CreateOrEditFiscalCalenderDto>(fiscalCalender) };
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FiscalCalenders_Delete)]
        public async Task DeleteCalendar(int input)
        {
            int? tenantId = (int?)AbpSession.TenantId;

            var lstFiscalCalender = await _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == tenantId && x.Period == input).Select(x => x).ToListAsync();

            foreach (var fc in lstFiscalCalender)
            {
                await _fiscalCalenderRepository.DeleteAsync(fc.Id);
            }
        }

        public async Task UpdateCalender(CreateOrEditFiscalCalenderDto input)
        {
            var fiscalCalender = ObjectMapper.Map<FiscalCalender>(input);

            if (AbpSession.TenantId != null)
                fiscalCalender.TenantId = (int?)AbpSession.TenantId;
            else
                fiscalCalender.TenantId = null;

            var lstFiscalCalender = await _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == fiscalCalender.TenantId && x.Period == fiscalCalender.Period).Select(x => x).ToListAsync();

            foreach (var fc in lstFiscalCalender)
            {
                fc.IsActive = fiscalCalender.IsActive;
                fc.IsLocked = fiscalCalender.IsLocked;
                var item = ObjectMapper.Map<CreateOrEditFiscalCalenderDto>(fc);
                await Update(item);
            }

        }
        public async Task CheckAll(int period, bool check)
        {
            int? TenantId = AbpSession.TenantId;
            var lstFiscalCalender = await _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == TenantId && x.Period == period).Select(x => x).ToListAsync();

            if (check)
            {
                foreach (var fc in lstFiscalCalender)
                {
                    fc.IsLocked = true;
                    fc.GL = true;
                    fc.AP = true;
                    fc.AR = true;
                    fc.IN = true;
                    fc.PO = true;
                    fc.OE = true;
                    fc.BK = true;
                    fc.HR = true;
                    fc.PR = true;

                    var item = ObjectMapper.Map<CreateOrEditFiscalCalenderDto>(fc);
                    await Update(item);
                }
            }
            else
            {
                foreach (var fc in lstFiscalCalender)
                {
                    fc.IsLocked = false;
                    fc.GL = false;
                    fc.AP = false;
                    fc.AR = false;
                    fc.IN = false;
                    fc.PO = false;
                    fc.OE = false;
                    fc.BK = false;
                    fc.HR = false;
                    fc.PR = false;

                    var item = ObjectMapper.Map<CreateOrEditFiscalCalenderDto>(fc);
                    await Update(item);
                }
            }


        }



        public int CheckCalendarStatus(int input)
        {
            int? tenantId = (int?)AbpSession.TenantId;
            var lstFiscalCalender = _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == tenantId && x.Period == input).Select(x => x).ToList();

            var bookCount = lstFiscalCalender.Where(x => x.AP == false || x.AR == false || x.BK == false || x.GL == false ||
                                                           x.HR == false || x.IN == false || x.OE == false || x.PO == false ||
                                                           x.PR == false).Select(x => x).ToList();
            return bookCount.Count;
        }

        public bool CalendarStatus(int? period)
        {
            int? tenantId = (int?)AbpSession.TenantId;
            var lstFiscalCalender = _fiscalCalenderRepository.GetAll().Where(x => x.TenantId == tenantId && x.IsActive == true && (x.Period != period || period == 0)).Select(x => x).ToList();

            if (lstFiscalCalender.Count > 0)
                return true;
            else
                return false;

        }

        public bool GetFiscalYearStatus(DateTime date, string fiscalYear)
        {
            var status = false;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlDataReader rdr = null;
                SqlCommand cmd = null;

                switch (fiscalYear)
                {
                    case "GL":
                        cmd = new SqlCommand("select top 1 GL from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate  and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["GL"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "AP":
                        cmd = new SqlCommand("select top 1 AP from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["AP"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "AR":
                        cmd = new SqlCommand("select top 1 AR from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["AR"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "IN":
                        cmd = new SqlCommand("select top 1 IN from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["IN"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "PO":
                        cmd = new SqlCommand("select top 1 PO from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["PO"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "BK":
                        cmd = new SqlCommand("select top 1 BK from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["BK"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "HR":
                        cmd = new SqlCommand("select top 1 HR from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["HR"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                    case "PR":
                        cmd = new SqlCommand("select top 1 PR from CSFISCALLOCK where '" + date + "'  between StartDate and EndDate   and TenantId = " + AbpSession.TenantId + " order by Id desc", cn);
                        cn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            status = Convert.ToBoolean(rdr["PR"]);
                        }
                        rdr.Close();
                        cmd.Dispose();
                        break;
                }
                //   // cn.Close();
            }
            var data = !status;
            return !status;
        }

        public DateTime GetFiscalDate()
        {
            var date = _fiscalCalenderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && o.IsActive == true
            ).FirstOrDefault().StartDate;
            return date;
        }
    }
}