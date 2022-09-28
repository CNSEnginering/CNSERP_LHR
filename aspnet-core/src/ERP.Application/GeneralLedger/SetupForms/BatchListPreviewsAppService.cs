

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
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_BatchListPreviews)]
    public class BatchListPreviewsAppService : ERPAppServiceBase, IBatchListPreviewsAppService
    {
        private readonly IRepository<BatchListPreview> _batchListPreviewRepository;
        private readonly IBatchListPreviewsExcelExporter _batchListPreviewsExcelExporter;
        private readonly IRepository<GLLocation> _glLocationRepository;

        private readonly IRepository<GLTRDetail> _gltrDetailRepository;

        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;


        public BatchListPreviewsAppService(IRepository<BatchListPreview> batchListPreviewRepository,
            IRepository<GLTRDetail> gltrDetailRepository, IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<GLTRHeader> gltrHeaderRepository, IRepository<GLBOOKS> glbooksRepository,
            IBatchListPreviewsExcelExporter batchListPreviewsExcelExporter, IRepository<GLLocation> glLocationRepository)
        {
            _batchListPreviewRepository = batchListPreviewRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _batchListPreviewsExcelExporter = batchListPreviewsExcelExporter;
            _gltrHeaderRepository = gltrHeaderRepository;
            _glbooksRepository = glbooksRepository;
            _glLocationRepository = glLocationRepository;
        }

        public async Task<PagedResultDto<GetBatchListPreviewForViewDto>> GetAll(GetAllBatchListPreviewsInput input)
        {
            var filteredBatchListPreviews = from header in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Posted==true)

                                            join detail in _gltrDetailRepository.GetAll() on new { header.Id, header.TenantId } equals new { Id = detail.DetID, detail.TenantId } into detail1
                                            from d1 in detail1.DefaultIfEmpty()

                                            join book in _glbooksRepository.GetAll() on new { header.BookID, header.TenantId } equals new { book.BookID, book.TenantId } into book1
                                            from b1 in book1.DefaultIfEmpty()

                                            join loc in _glLocationRepository.GetAll() on new { header.LocId, header.TenantId } equals new { loc.LocId, loc.TenantId } into loc1
                                            from l1 in loc1.DefaultIfEmpty()

                                            select new
                                            {

                                                header.FmtDocNo,
                                                d1.Amount,
                                                b1.BookName,
                                                header.NARRATION,
                                                header.DocDate,
                                                header.BookID,
                                                header.DocMonth,
                                                header.Approved,
                                                header.Posted,
                                                header.Reference,
                                                l1.LocDesc,
                                                header.Id

                                            };




            filteredBatchListPreviews = filteredBatchListPreviews
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NARRATION.Contains(input.Filter) || e.BookID.Contains(input.Filter) || e.BookName.Contains(input.Filter))
                .WhereIf(input.minVoucherNoFilter != null, e => e.FmtDocNo >= input.minVoucherNoFilter)
                .WhereIf(input.maxVoucherNoFilter != null, e => e.FmtDocNo <= input.maxVoucherNoFilter)
                .WhereIf(input.Status != -1, e => Convert.ToInt32(e.Approved) == input.Status)
                .WhereIf(input.PostedFilter != -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                .WhereIf(input.StatusFilter == "Approved", e => e.Approved == true)
                .WhereIf(input.StatusFilter == "Posted", e => e.Posted == true)
                .WhereIf(input.StatusFilter == "Unapproved", e => e.Approved == false)

                .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.LocDesc.ToLower() == input.LocationFilter.ToLower().Trim())
                .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceFilter), e => e.Reference.ToLower() == input.ReferenceFilter.ToLower().Trim())
                .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.NARRATION.ToLower() == input.NarrationFilter.ToLower().Trim())
                .WhereIf(input.minAmountFilter != null, e => e.Amount >= input.minAmountFilter)
                .WhereIf(input.maxAmountFilter != null, e => e.Amount <= input.maxAmountFilter)

                .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                .WhereIf(input.MinDocDateFilter != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) >= Convert.ToDateTime(Convert.ToDateTime(input.MinDocDateFilter.ToString()).ToShortDateString()))
                .WhereIf(input.MaxDocDateFilter != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) <= Convert.ToDateTime(Convert.ToDateTime(input.MaxDocDateFilter.ToString()).ToShortDateString()));

            var batchListPreviewsSum = from o in filteredBatchListPreviews
                                       group o by new
                                       {
                                           o.FmtDocNo,
                                           o.BookID,
                                           o.BookName,
                                           o.DocDate,
                                           o.NARRATION,
                                           o.DocMonth,
                                           o.Approved,
                                           o.Reference,
                                           o.LocDesc,
                                           o.Id

                                       } into g

                                       select new
                                       {

                                           g.Key.FmtDocNo,
                                           g.Key.BookID,
                                           g.Key.DocDate,
                                           g.Key.DocMonth,
                                           g.Key.NARRATION,
                                           g.Key.BookName,
                                           g.Key.Approved,
                                           g.Key.Reference,
                                           g.Key.LocDesc,
                                           g.Key.Id,
                                           Debit = g.Where(x => x.Amount > 0).Sum(x => x.Amount),
                                           Credit = g.Where(x => x.Amount < 0).Sum(x => Math.Abs((decimal)x.Amount))

                                       };

            var pagedandSortedResult = batchListPreviewsSum.OrderBy(input.Sorting ?? "Id desc").PageBy(input);


            var batchListPreviews = from o in pagedandSortedResult
                                    select new GetBatchListPreviewForViewDto()
                                    {
                                        BatchListPreview = new BatchListPreviewDto
                                        {
                                            BookID = o.BookID,
                                            DocDate = o.DocDate,
                                            DocMonth = o.DocMonth,
                                            Description = o.NARRATION,
                                            BookDesc = o.BookName,
                                            Debit = (double)o.Debit,
                                            Credit = (double)o.Credit,
                                            Approved = o.Approved,
                                            Reference = o.Reference,
                                            LocDesc = o.LocDesc,
                                            Id = o.FmtDocNo
                                        }
                                    };



            var totalCount = await batchListPreviewsSum.CountAsync();

            return new PagedResultDto<GetBatchListPreviewForViewDto>(
                totalCount,
                await batchListPreviews.ToListAsync()
            );
        }

        public async Task<ICollection<GetBatchListPreviewForViewDto>> GetBatchListPreviewForView(int id, string bookpID, DateTime docDate)
        {

            var JournalVoucherHeader = _gltrHeaderRepository.GetAll().Where(x => x.FmtDocNo == id && x.BookID == bookpID && x.TenantId == AbpSession.TenantId
            && x.DocDate.Month == docDate.Month && x.DocDate.Year == docDate.Year
            );

            var batchListPreviewDetails = from header in JournalVoucherHeader

                                          join detail in _gltrDetailRepository.GetAll() on new { header.Id, header.TenantId } equals new { Id = detail.DetID, detail.TenantId } into detail1
                                          from d1 in detail1.DefaultIfEmpty()

                                          join book in _glbooksRepository.GetAll() on new { header.BookID, header.TenantId } equals new { book.BookID, book.TenantId } into book1
                                          from b1 in book1.DefaultIfEmpty()

                                          join account in _chartofControlRepository.GetAll() on new { Id = d1.AccountID, d1.TenantId } equals new { account.Id, account.TenantId }
                                     into account1
                                          from a1 in account1.DefaultIfEmpty()

                                              //where header.DocNo == id

                                          select new GetBatchListPreviewForViewDto()
                                          {
                                              BatchListPreview = new BatchListPreviewDto
                                              {
                                                  BookID = header.BookID,
                                                  DocDate = header.DocDate,
                                                  DocMonth = header.DocMonth,
                                                  BookDesc = b1.BookName,
                                                  Id = header.DocNo

                                              },
                                              gLTRDetailDto = new GLTRDetailDto
                                              {
                                                  Id = d1.DetID,
                                                  AccountID = d1.AccountID,
                                                  Narration = d1.Narration,
                                                  Amount = d1.Amount,
                                                  ChequeNo = d1.ChequeNo
                                              }
                                         ,
                                              chartofControlDto = new ChartofControlDto
                                              {
                                                  AccountName = a1.AccountName
                                              }
                                          };

            //var batchListPreview =  _batchListPreviewRepository.GetAll().Where(x => x.Id == id);
            //var batchListPreviewDetails = from o in batchListPreview

            //                              join o1 in _gltrDetailRepository.GetAll() on o.Id equals o1.DetID into j1
            //                              from s1 in j1.DefaultIfEmpty()

            //                              join o2 in _chartofControlRepository.GetAll() on s1.AccountID equals o2.Id into j2
            //                              from s2 in j2.DefaultIfEmpty()

            //                              select new GetBatchListPreviewForViewDto()
            //                              {
            //                                  gLTRDetailDto = new GLTRDetailDto
            //                                  {
            //                                      Id = s1.DetID,
            //                                      AccountID = s1.AccountID,
            //                                      Narration = s1.Narration,
            //                                      Amount = s1.Amount,
            //                                      ChequeNo = s1.ChequeNo
            //                                  }
            //                                  ,

            //                                  BatchListPreview = new BatchListPreviewDto
            //                                  {
            //                                      Id = o.Id,
            //                                      BookDesc = o.BookDesc,
            //                                      DocDate = o.DocDate,
            //                                  },

            //                                  chartofControlDto = new ChartofControlDto {
            //                                      AccountName = s2.AccountName
            //                                  }
            //                              };

            var totalCount = await batchListPreviewDetails.CountAsync();


            var batchListPreviewListDtos = await batchListPreviewDetails.ToListAsync();

            return batchListPreviewListDtos;
        }

        [AbpAuthorize(AppPermissions.Pages_BatchListPreviews_Edit)]
        public async Task<GetBatchListPreviewForEditOutput> GetBatchListPreviewForEdit(EntityDto input)
        {
            var batchListPreview = await _batchListPreviewRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetBatchListPreviewForEditOutput { BatchListPreview = ObjectMapper.Map<CreateOrEditBatchListPreviewDto>(batchListPreview) };
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBatchListPreviewDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_BatchListPreviews_Create)]
        protected virtual async Task Create(CreateOrEditBatchListPreviewDto input)
        {
            var batchListPreview = ObjectMapper.Map<BatchListPreview>(input);


            //if (AbpSession.TenantId != null)
            //{
            //	batchListPreview.TenantId = (int?) AbpSession.TenantId;
            //}


            await _batchListPreviewRepository.InsertAsync(batchListPreview);
        }

        [AbpAuthorize(AppPermissions.Pages_BatchListPreviews_Edit)]
        protected virtual async Task Update(CreateOrEditBatchListPreviewDto input)
        {
            var batchListPreview = await _batchListPreviewRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, batchListPreview);
        }

        [AbpAuthorize(AppPermissions.Pages_BatchListPreviews_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _batchListPreviewRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBatchListPreviewsToExcel(GetAllBatchListPreviewsForExcelInput input)
        {

            var filteredBatchListPreviews = _batchListPreviewRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.BookDesc.Contains(input.Filter));

            var query = (from o in filteredBatchListPreviews
                         select new GetBatchListPreviewForViewDto()
                         {
                             BatchListPreview = new BatchListPreviewDto
                             {
                                 DocDate = o.DocDate,
                                 DocMonth = o.DocMonth,
                                 Description = o.Description,
                                 BookDesc = o.BookDesc,
                                 Debit = o.Debit,
                                 Credit = o.Credit,
                                 Id = o.Id
                             }
                         });


            var batchListPreviewListDtos = await query.ToListAsync();
            return _batchListPreviewsExcelExporter.ExportToFile(batchListPreviewListDtos);
        }


    }
}