

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
    [AbpAuthorize(AppPermissions.Pages_ChequeBooks)]
    public class ChequeBooksAppService : ERPAppServiceBase, IChequeBooksAppService
    {
        private readonly IRepository<ChequeBook> _chequeBookRepository;
        private readonly IChequeBooksExcelExporter _chequeBooksExcelExporter;
        private readonly IChequeBookDetailsAppService _chequeBookDetailsAppService;
        private readonly IRepository<Bank, int> _bankRepository;



        public ChequeBooksAppService(IRepository<ChequeBook> chequeBookRepository, IChequeBooksExcelExporter chequeBooksExcelExporter,
            IChequeBookDetailsAppService chequeBookDetailsAppService, IRepository<Bank, int> bankRepository)
        {
            _chequeBookRepository = chequeBookRepository;
            _chequeBooksExcelExporter = chequeBooksExcelExporter;
            _chequeBookDetailsAppService = chequeBookDetailsAppService;
            _bankRepository = bankRepository;
        }

        public async Task<PagedResultDto<GetChequeBookForViewDto>> GetAll(GetAllChequeBooksInput input)
        {

            var filteredChequeBooks = _chequeBookRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BANKID.Contains(input.Filter) || e.BankAccNo.Contains(input.Filter) || e.FromChNo.Contains(input.Filter) || e.ToChNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKIDFilter), e => e.BANKID == input.BANKIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccNoFilter), e => e.BankAccNo == input.BankAccNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FromChNoFilter), e => e.FromChNo == input.FromChNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ToChNoFilter), e => e.ToChNo == input.ToChNoFilter)
                        .WhereIf(input.MinNoofChFilter != null, e => e.NoofCh >= input.MinNoofChFilter)
                        .WhereIf(input.MaxNoofChFilter != null, e => e.NoofCh <= input.MaxNoofChFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredChequeBooks = filteredChequeBooks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var chequeBooks = from o in pagedAndFilteredChequeBooks
                              select new GetChequeBookForViewDto()
                              {
                                  ChequeBook = new ChequeBookDto
                                  {
                                      DocNo = o.DocNo,
                                      DocDate = o.DocDate,
                                      BANKID = o.BANKID,
                                      BankAccNo = o.BankAccNo,
                                      FromChNo = o.FromChNo,
                                      ToChNo = o.ToChNo,
                                      NoofCh = o.NoofCh,
                                      Active = o.Active,
                                      AudtUser = o.AudtUser,
                                      AudtDate = o.AudtDate,
                                      CreatedBy = o.CreatedBy,
                                      CreateDate = o.CreateDate,
                                      Id = o.Id
                                  }
                              };

            var totalCount = await filteredChequeBooks.CountAsync();

            return new PagedResultDto<GetChequeBookForViewDto>(
                totalCount,
                await chequeBooks.ToListAsync()
            );
        }

        public async Task<GetChequeBookForViewDto> GetChequeBookForView(int id)
        {
            var chequeBook = await _chequeBookRepository.GetAsync(id);

            var output = new GetChequeBookForViewDto { ChequeBook = ObjectMapper.Map<ChequeBookDto>(chequeBook) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBooks_Edit)]
        public async Task<GetChequeBookForEditOutput> GetChequeBookForEdit(EntityDto input)
        {
            var chequeBook = await _chequeBookRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetChequeBookForEditOutput { ChequeBook = ObjectMapper.Map<CreateOrEditChequeBookDto>(chequeBook) };

            var chequeBookDetail = await _chequeBookDetailsAppService.GetChequeBookDetailForEdit((int)output.ChequeBook.Id);
            output.ChequeBook.ChequeBookDetail = chequeBookDetail.ChequeBookDetail;

            output.ChequeBook.BankName = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == output.ChequeBook.BANKID).Select(p => p.BANKNAME).FirstOrDefault();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditChequeBookDto input)
        {
            if (!input.flag)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBooks_Create)]
        protected virtual async Task Create(CreateOrEditChequeBookDto input)
        {
            var chequeBook = ObjectMapper.Map<ChequeBook>(input);

            if (AbpSession.TenantId != null)
            {
                chequeBook.TenantId = (int)AbpSession.TenantId;
            }

            chequeBook.DocNo = GetMaxID();
            int Hid = await _chequeBookRepository.InsertAndGetIdAsync(chequeBook);

            foreach (var item in input.ChequeBookDetail)
            {
                item.DetID = Hid;
                item.BankAccNo = input.BankAccNo;
            }

            await _chequeBookDetailsAppService.CreateOrEdit(input.ChequeBookDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBooks_Edit)]
        protected virtual async Task Update(CreateOrEditChequeBookDto input)
        {
            var chequeBook = await _chequeBookRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, chequeBook);

            foreach (var item in input.ChequeBookDetail)
            {
                item.DetID = (int)input.Id;
                item.BankAccNo = input.BankAccNo;
            }

            await _chequeBookDetailsAppService.Delete((int)input.Id);
            await _chequeBookDetailsAppService.CreateOrEdit(input.ChequeBookDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_ChequeBooks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _chequeBookRepository.DeleteAsync(input.Id);
            await _chequeBookDetailsAppService.Delete(input.Id);
        }

        public async Task<FileDto> GetChequeBooksToExcel(GetAllChequeBooksForExcelInput input)
        {

            var filteredChequeBooks = _chequeBookRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BANKID.Contains(input.Filter) || e.BankAccNo.Contains(input.Filter) || e.FromChNo.Contains(input.Filter) || e.ToChNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKIDFilter), e => e.BANKID == input.BANKIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccNoFilter), e => e.BankAccNo == input.BankAccNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FromChNoFilter), e => e.FromChNo == input.FromChNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ToChNoFilter), e => e.ToChNo == input.ToChNoFilter)
                        .WhereIf(input.MinNoofChFilter != null, e => e.NoofCh >= input.MinNoofChFilter)
                        .WhereIf(input.MaxNoofChFilter != null, e => e.NoofCh <= input.MaxNoofChFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredChequeBooks
                         select new GetChequeBookForViewDto()
                         {
                             ChequeBook = new ChequeBookDto
                             {
                                 DocNo = o.DocNo,
                                 DocDate = o.DocDate,
                                 BANKID = o.BANKID,
                                 BankAccNo = o.BankAccNo,
                                 FromChNo = o.FromChNo,
                                 ToChNo = o.ToChNo,
                                 NoofCh = o.NoofCh,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var chequeBookListDtos = await query.ToListAsync();

            return _chequeBooksExcelExporter.ExportToFile(chequeBookListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _chequeBookRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            return maxid;
        }

    }
}