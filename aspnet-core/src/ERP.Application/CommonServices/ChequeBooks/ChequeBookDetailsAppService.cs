

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.ChequeBooks.Exporting;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.CommonServices.ChequeBooks
{
    [AbpAuthorize(AppPermissions.Pages_ChequeBookDetails)]
    public class ChequeBookDetailsAppService : ERPAppServiceBase, IChequeBookDetailsAppService
    {
        private readonly IRepository<ChequeBookDetail> _chequeBookDetailRepository;
        private readonly IChequeBookDetailsExcelExporter _chequeBookDetailsExcelExporter;


        public ChequeBookDetailsAppService(IRepository<ChequeBookDetail> chequeBookDetailRepository, IChequeBookDetailsExcelExporter chequeBookDetailsExcelExporter)
        {
            _chequeBookDetailRepository = chequeBookDetailRepository;
            _chequeBookDetailsExcelExporter = chequeBookDetailsExcelExporter;

        }

        public async Task<PagedResultDto<GetChequeBookDetailForViewDto>> GetAll(GetAllChequeBookDetailsInput input)
        {

            var filteredChequeBookDetails = _chequeBookDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BANKID.Contains(input.Filter) || e.BankAccNo.Contains(input.Filter) || e.FromChNo.Contains(input.Filter) || e.ToChNo.Contains(input.Filter) || e.BooKID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKIDFilter), e => e.BANKID == input.BANKIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccNoFilter), e => e.BankAccNo == input.BankAccNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FromChNoFilter), e => e.FromChNo == input.FromChNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ToChNoFilter), e => e.ToChNo == input.ToChNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BooKIDFilter), e => e.BooKID == input.BooKIDFilter)
                        .WhereIf(input.MinVoucherNoFilter != null, e => e.VoucherNo >= input.MinVoucherNoFilter)
                        .WhereIf(input.MaxVoucherNoFilter != null, e => e.VoucherNo <= input.MaxVoucherNoFilter)
                        .WhereIf(input.MinVoucherDateFilter != null, e => e.VoucherDate >= input.MinVoucherDateFilter)
                        .WhereIf(input.MaxVoucherDateFilter != null, e => e.VoucherDate <= input.MaxVoucherDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredChequeBookDetails = filteredChequeBookDetails
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var chequeBookDetails = from o in pagedAndFilteredChequeBookDetails
                                    select new GetChequeBookDetailForViewDto()
                                    {
                                        ChequeBookDetail = new ChequeBookDetailDto
                                        {
                                            DetID = o.DetID,
                                            DocNo = o.DocNo,
                                            BANKID = o.BANKID,
                                            BankAccNo = o.BankAccNo,
                                            FromChNo = o.FromChNo,
                                            ToChNo = o.ToChNo,
                                            BooKID = o.BooKID,
                                            VoucherNo = o.VoucherNo,
                                            VoucherDate = o.VoucherDate,
                                            Narration = o.Narration,
                                            AudtUser = o.AudtUser,
                                            AudtDate = o.AudtDate,
                                            CreatedBy = o.CreatedBy,
                                            CreateDate = o.CreateDate,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredChequeBookDetails.CountAsync();

            return new PagedResultDto<GetChequeBookDetailForViewDto>(
                totalCount,
                await chequeBookDetails.ToListAsync()
            );
        }

        public async Task<GetChequeBookDetailForViewDto> GetChequeBookDetailForView(int id)
        {
            var chequeBookDetail = await _chequeBookDetailRepository.GetAsync(id);

            var output = new GetChequeBookDetailForViewDto { ChequeBookDetail = ObjectMapper.Map<ChequeBookDetailDto>(chequeBookDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBookDetails_Edit)]
        public async Task<GetChequeBookDetailForEditOutput> GetChequeBookDetailForEdit(int ID)
        {
            var chequeBookDetail = await _chequeBookDetailRepository.GetAllListAsync(x => x.DetID == ID && x.TenantId == AbpSession.TenantId);

            var chequeBookDetails = from o in chequeBookDetail
                                    select new CreateOrEditChequeBookDetailDto
                                    {
                                        DetID = o.DetID,
                                        DocNo = o.DocNo,
                                        BANKID = o.BANKID,
                                        BankAccNo = o.BankAccNo,
                                        FromChNo = o.FromChNo,
                                        ToChNo = o.ToChNo,
                                        BooKID = o.BooKID,
                                        VoucherNo = o.VoucherNo,
                                        VoucherDate = o.VoucherDate,
                                        Narration = o.Narration,
                                        AudtUser = o.AudtUser,
                                        AudtDate = o.AudtDate,
                                        CreatedBy = o.CreatedBy,
                                        CreateDate = o.CreateDate,
                                        Id = o.Id
                                    };

            var output = new GetChequeBookDetailForEditOutput { ChequeBookDetail = ObjectMapper.Map<ICollection<CreateOrEditChequeBookDetailDto>>(chequeBookDetail) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditChequeBookDetailDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBookDetails_Create)]
        protected virtual async Task Create(CreateOrEditChequeBookDetailDto input)
        {
            ChequeBookDetail chequeBookDetail = new ChequeBookDetail();
            chequeBookDetail.DetID = (int)input.DetID;
            chequeBookDetail.DocNo = input.DocNo;
            chequeBookDetail.BANKID = input.BANKID;
            chequeBookDetail.BankAccNo = input.BankAccNo;
            chequeBookDetail.FromChNo = input.FromChNo;
            chequeBookDetail.ToChNo = input.ToChNo;
            chequeBookDetail.BooKID = input.BooKID;
            chequeBookDetail.VoucherNo = input.VoucherNo;
            chequeBookDetail.VoucherDate = input.VoucherDate;
            chequeBookDetail.Narration = input.Narration;
            chequeBookDetail.AudtUser = input.AudtUser;
            chequeBookDetail.AudtDate = input.AudtDate;
            chequeBookDetail.CreatedBy = input.CreatedBy;
            chequeBookDetail.CreateDate = input.CreateDate;

            if (AbpSession.TenantId != null)
            {
                chequeBookDetail.TenantId = (int)AbpSession.TenantId;
            }

            await _chequeBookDetailRepository.InsertAsync(chequeBookDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBookDetails_Edit)]
        protected virtual async Task Update(CreateOrEditChequeBookDetailDto input)
        {
            ChequeBookDetail chequeBookDetail = new ChequeBookDetail();
            chequeBookDetail.DetID = (int)input.DetID;
            chequeBookDetail.DocNo = input.DocNo;
            chequeBookDetail.BANKID = input.BANKID;
            chequeBookDetail.BankAccNo = input.BankAccNo;
            chequeBookDetail.FromChNo = input.FromChNo;
            chequeBookDetail.ToChNo = input.ToChNo;
            chequeBookDetail.BooKID = input.BooKID;
            chequeBookDetail.VoucherNo = input.VoucherNo;
            chequeBookDetail.VoucherDate = input.VoucherDate;
            chequeBookDetail.Narration = input.Narration;
            chequeBookDetail.AudtUser = input.AudtUser;
            chequeBookDetail.AudtDate = input.AudtDate;
            chequeBookDetail.CreatedBy = input.CreatedBy;
            chequeBookDetail.CreateDate = input.CreateDate;

            await _chequeBookDetailRepository.InsertAsync(chequeBookDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBookDetails_Delete)]
        public async Task Delete(int input)
        {
            await _chequeBookDetailRepository.DeleteAsync(x => x.DetID == input);
        }

        public async Task<FileDto> GetChequeBookDetailsToExcel(GetAllChequeBookDetailsForExcelInput input)
        {

            var filteredChequeBookDetails = _chequeBookDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BANKID.Contains(input.Filter) || e.BankAccNo.Contains(input.Filter) || e.FromChNo.Contains(input.Filter) || e.ToChNo.Contains(input.Filter) || e.BooKID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKIDFilter), e => e.BANKID == input.BANKIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccNoFilter), e => e.BankAccNo == input.BankAccNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FromChNoFilter), e => e.FromChNo == input.FromChNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ToChNoFilter), e => e.ToChNo == input.ToChNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BooKIDFilter), e => e.BooKID == input.BooKIDFilter)
                        .WhereIf(input.MinVoucherNoFilter != null, e => e.VoucherNo >= input.MinVoucherNoFilter)
                        .WhereIf(input.MaxVoucherNoFilter != null, e => e.VoucherNo <= input.MaxVoucherNoFilter)
                        .WhereIf(input.MinVoucherDateFilter != null, e => e.VoucherDate >= input.MinVoucherDateFilter)
                        .WhereIf(input.MaxVoucherDateFilter != null, e => e.VoucherDate <= input.MaxVoucherDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredChequeBookDetails
                         select new GetChequeBookDetailForViewDto()
                         {
                             ChequeBookDetail = new ChequeBookDetailDto
                             {
                                 DetID = o.DetID,
                                 DocNo = o.DocNo,
                                 BANKID = o.BANKID,
                                 BankAccNo = o.BankAccNo,
                                 FromChNo = o.FromChNo,
                                 ToChNo = o.ToChNo,
                                 BooKID = o.BooKID,
                                 VoucherNo = o.VoucherNo,
                                 VoucherDate = o.VoucherDate,
                                 Narration = o.Narration,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var chequeBookDetailListDtos = await query.ToListAsync();

            return _chequeBookDetailsExcelExporter.ExportToFile(chequeBookDetailListDtos);
        }


    }
}