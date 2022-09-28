

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Allowances.Exporting;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.Allowances
{
    [AbpAuthorize(AppPermissions.PayRoll_AllowancesDetails)]
    public class AllowancesDetailsAppService : ERPAppServiceBase, IAllowancesDetailsAppService
    {
        private readonly IRepository<AllowancesDetail> _allowancesDetailRepository;
        private readonly IAllowancesDetailsExcelExporter _allowancesDetailsExcelExporter;
        private readonly IRepository<Employees.Employees> _employeesRepository;



        public AllowancesDetailsAppService(IRepository<AllowancesDetail> allowancesDetailRepository, IAllowancesDetailsExcelExporter allowancesDetailsExcelExporter,
            IRepository<Employees.Employees> employeesRepository)
        {
            _allowancesDetailRepository = allowancesDetailRepository;
            _allowancesDetailsExcelExporter = allowancesDetailsExcelExporter;
            _employeesRepository = employeesRepository;
        }

        public async Task<PagedResultDto<GetAllowancesDetailForViewDto>> GetAll(GetAllAllowancesDetailsInput input)
        {

            var filteredAllowancesDetails = _allowancesDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(input.MinAllowanceTypeFilter != null, e => e.AllowanceType >= input.MinAllowanceTypeFilter)
                        .WhereIf(input.MaxAllowanceTypeFilter != null, e => e.AllowanceType <= input.MaxAllowanceTypeFilter)
                        .WhereIf(input.MinAllowanceAmtFilter != null, e => e.AllowanceAmt >= input.MinAllowanceAmtFilter)
                        .WhereIf(input.MaxAllowanceAmtFilter != null, e => e.AllowanceAmt <= input.MaxAllowanceAmtFilter)
                        .WhereIf(input.MinAllowanceQtyFilter != null, e => e.AllowanceQty >= input.MinAllowanceQtyFilter)
                        .WhereIf(input.MaxAllowanceQtyFilter != null, e => e.AllowanceQty <= input.MaxAllowanceQtyFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAllowancesDetails = filteredAllowancesDetails
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var allowancesDetails = from o in pagedAndFilteredAllowancesDetails
                                    select new GetAllowancesDetailForViewDto()
                                    {
                                        AllowancesDetail = new AllowancesDetailDto
                                        {
                                            DetID = o.DetID,
                                            EmployeeID = o.EmployeeID,
                                            AllowanceType = o.AllowanceType,
                                            AllowanceAmt = o.AllowanceAmt,
                                            AllowanceQty = o.AllowanceQty,
                                            Amount = o.Amount,
                                            AudtUser = o.AudtUser,
                                            AudtDate = o.AudtDate,
                                            CreatedBy = o.CreatedBy,
                                            CreateDate = o.CreateDate,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredAllowancesDetails.CountAsync();

            return new PagedResultDto<GetAllowancesDetailForViewDto>(
                totalCount,
                await allowancesDetails.ToListAsync()
            );
        }

        public async Task<GetAllowancesDetailForViewDto> GetAllowancesDetailForView(int id)
        {
            var allowancesDetail = await _allowancesDetailRepository.GetAsync(id);

            var output = new GetAllowancesDetailForViewDto { AllowancesDetail = ObjectMapper.Map<AllowancesDetailDto>(allowancesDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowancesDetails_Edit)]
        public async Task<GetAllowancesDetailForEditOutput> GetAllowancesDetailForEdit(int ID)
        {
            var allowancesDetail = await _allowancesDetailRepository.GetAllListAsync(x => x.DetID == ID && x.TenantId == AbpSession.TenantId);
            var employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Active == true);


            var allowancesDetails = from o in allowancesDetail
                                    join b in employeesData
                                    on o.EmployeeID equals b.EmployeeID
                                    select new CreateOrEditAllowancesDetailDto
                                    {
                                        DetID = o.DetID,
                                        EmployeeID = o.EmployeeID,
                                        EmployeeName = b.EmployeeName,
                                        AllowanceType = o.AllowanceType,
                                        AllowanceAmt = o.AllowanceAmt,
                                        AllowanceQty = o.AllowanceQty,
                                        Milage = o.Milage,
                                        ParkingFees = o.ParkingFees,
                                        Amount = o.Amount,
                                        AudtUser = o.AudtUser,
                                        AudtDate = o.AudtDate,
                                        CreatedBy = o.CreatedBy,
                                        CreateDate = o.CreateDate,
                                        Id = o.Id,
                                        AllowanceTypeName = o.AllowanceTypeName,
                                        PerlitrMilg = o.PerlitrMilg,
                                        RepairRate=o.RepairRate
                                    };

            var output = new GetAllowancesDetailForEditOutput { AllowancesDetail = ObjectMapper.Map<ICollection<CreateOrEditAllowancesDetailDto>>(allowancesDetails) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditAllowancesDetailDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null || item.Id == 0)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowancesDetails_Create)]
        protected virtual async Task Create(CreateOrEditAllowancesDetailDto input)
        {
            AllowancesDetail allowancesDetail = new AllowancesDetail();
            allowancesDetail.DetID = input.DetID;
            allowancesDetail.EmployeeID = input.EmployeeID;
            allowancesDetail.AllowanceType = input.AllowanceType;
            allowancesDetail.AllowanceAmt = input.AllowanceAmt;
            allowancesDetail.AllowanceQty = input.AllowanceQty;
            allowancesDetail.Milage = input.Milage;
            allowancesDetail.ParkingFees = input.ParkingFees;
            allowancesDetail.Amount = input.Amount;
            allowancesDetail.AudtUser = input.AudtUser;
            allowancesDetail.AudtDate = input.AudtDate;
            allowancesDetail.CreatedBy = await GetCurrentUserName();
            allowancesDetail.CreateDate = DateTime.Now;
            allowancesDetail.PerlitrMilg = input.PerlitrMilg;
            allowancesDetail.RepairRate = input.RepairRate;
            allowancesDetail.AllowanceTypeName = input.AllowanceTypeName;
            if (AbpSession.TenantId != null)
            {
                allowancesDetail.TenantId = (int)AbpSession.TenantId;
            }

            await _allowancesDetailRepository.InsertAsync(allowancesDetail);
        }

        private async Task<string> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user.UserName;
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowancesDetails_Edit)]
        protected virtual async Task Update(CreateOrEditAllowancesDetailDto input)
        {
            AllowancesDetail allowancesDetail = new AllowancesDetail();
            allowancesDetail.DetID = input.DetID;
            allowancesDetail.EmployeeID = input.EmployeeID;
            allowancesDetail.AllowanceType = input.AllowanceType;
            allowancesDetail.AllowanceAmt = input.AllowanceAmt;
            allowancesDetail.AllowanceQty = input.AllowanceQty;
            allowancesDetail.Milage = input.Milage;
            allowancesDetail.ParkingFees = input.ParkingFees;
            allowancesDetail.Amount = input.Amount;
            allowancesDetail.AudtUser = input.AudtUser;
            allowancesDetail.AudtDate = input.AudtDate;
            allowancesDetail.CreatedBy = input.CreatedBy;
            allowancesDetail.CreateDate = input.CreateDate;
            allowancesDetail.PerlitrMilg = input.PerlitrMilg;
            allowancesDetail.RepairRate = input.RepairRate;
            allowancesDetail.AllowanceTypeName = input.AllowanceTypeName;
            if (AbpSession.TenantId != null)
            {
                allowancesDetail.TenantId = (int)AbpSession.TenantId;
            }

            await _allowancesDetailRepository.InsertAsync(allowancesDetail);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowancesDetails_Delete)]
        public async Task Delete(int input)
        {
            await _allowancesDetailRepository.DeleteAsync(x => x.DetID == input);
        }

        public async Task<FileDto> GetAllowancesDetailsToExcel(GetAllAllowancesDetailsForExcelInput input)
        {

            var filteredAllowancesDetails = _allowancesDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(input.MinAllowanceTypeFilter != null, e => e.AllowanceType >= input.MinAllowanceTypeFilter)
                        .WhereIf(input.MaxAllowanceTypeFilter != null, e => e.AllowanceType <= input.MaxAllowanceTypeFilter)
                        .WhereIf(input.MinAllowanceAmtFilter != null, e => e.AllowanceAmt >= input.MinAllowanceAmtFilter)
                        .WhereIf(input.MaxAllowanceAmtFilter != null, e => e.AllowanceAmt <= input.MaxAllowanceAmtFilter)
                        .WhereIf(input.MinAllowanceQtyFilter != null, e => e.AllowanceQty >= input.MinAllowanceQtyFilter)
                        .WhereIf(input.MaxAllowanceQtyFilter != null, e => e.AllowanceQty <= input.MaxAllowanceQtyFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredAllowancesDetails
                         select new GetAllowancesDetailForViewDto()
                         {
                             AllowancesDetail = new AllowancesDetailDto
                             {
                                 DetID = o.DetID,
                                 EmployeeID = o.EmployeeID,
                                 AllowanceType = o.AllowanceType,
                                 AllowanceAmt = o.AllowanceAmt,
                                 AllowanceQty = o.AllowanceQty,
                                 Amount = o.Amount,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var allowancesDetailListDtos = await query.ToListAsync();

            return _allowancesDetailsExcelExporter.ExportToFile(allowancesDetailListDtos);
        }


    }
}