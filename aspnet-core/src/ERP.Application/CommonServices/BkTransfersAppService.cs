using ERP.CommonServices;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.Exporting;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;

namespace ERP.CommonServices
{
    [AbpAuthorize(AppPermissions.Pages_BkTransfers)]
    public class BkTransfersAppService : ERPAppServiceBase, IBkTransfersAppService
    {
        private readonly IRepository<BkTransfer> _bkTransferRepository;
        private readonly IBkTransfersExcelExporter _bkTransfersExcelExporter;
        private readonly IRepository<Bank, int> _lookup_bankRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<User, long> _userRepository;

        private VoucherEntryAppService _voucherEntryAppService;

        public BkTransfersAppService(IRepository<BkTransfer> bkTransferRepository, IBkTransfersExcelExporter bkTransfersExcelExporter,
              IRepository<Bank, int> lookup_bankRepository,
              IRepository<User, long> userRepository,
              IRepository<GLTRHeader> repository,
              IRepository<GLTRDetail> detailRepository,
              VoucherEntryAppService voucherEntryAppService
              )
        {
            _bkTransferRepository = bkTransferRepository;
            _bkTransfersExcelExporter = bkTransfersExcelExporter;
            _lookup_bankRepository = lookup_bankRepository;
            _userRepository = userRepository;
            _gltrHeaderRepository = repository;
            _gltrDetailRepository = detailRepository;
            _voucherEntryAppService = voucherEntryAppService;
        }

        public async Task<PagedResultDto<GetBkTransferForViewDto>> GetAll(GetAllBkTransfersInput input)
        {

            var filteredBkTransfers = _bkTransferRepository.GetAll()
                        //.Include( e => e.BankFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CMPID.Contains(input.Filter) || e.DESCRIPTION.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CMPIDFilter), e => e.CMPID.ToLower() == input.CMPIDFilter.ToLower().Trim())
                        .WhereIf(input.MinDOCIDFilter != null, e => e.DOCID >= input.MinDOCIDFilter)
                        .WhereIf(input.MaxDOCIDFilter != null, e => e.DOCID <= input.MaxDOCIDFilter)
                        .WhereIf(input.MinDOCDATEFilter != null, e => e.DOCDATE >= input.MinDOCDATEFilter)
                        .WhereIf(input.MaxDOCDATEFilter != null, e => e.DOCDATE <= input.MaxDOCDATEFilter)
                        .WhereIf(input.MinTRANSFERDATEFilter != null, e => e.TRANSFERDATE >= input.MinTRANSFERDATEFilter)
                        .WhereIf(input.MaxTRANSFERDATEFilter != null, e => e.TRANSFERDATE <= input.MaxTRANSFERDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DESCRIPTIONFilter), e => e.DESCRIPTION.ToLower() == input.DESCRIPTIONFilter.ToLower().Trim())
                        .WhereIf(input.MinFROMBANKIDFilter != null, e => e.FROMBANKID >= input.MinFROMBANKIDFilter)
                        .WhereIf(input.MaxFROMBANKIDFilter != null, e => e.FROMBANKID <= input.MaxFROMBANKIDFilter)
                        .WhereIf(input.MinFROMCONFIGIDFilter != null, e => e.FROMCONFIGID >= input.MinFROMCONFIGIDFilter)
                        .WhereIf(input.MaxFROMCONFIGIDFilter != null, e => e.FROMCONFIGID <= input.MaxFROMCONFIGIDFilter)
                        .WhereIf(input.MinTOBANKIDFilter != null, e => e.TOBANKID >= input.MinTOBANKIDFilter)
                        .WhereIf(input.MaxTOBANKIDFilter != null, e => e.TOBANKID <= input.MaxTOBANKIDFilter)
                        .WhereIf(input.MinTOCONFIGIDFilter != null, e => e.TOCONFIGID >= input.MinTOCONFIGIDFilter)
                        .WhereIf(input.MaxTOCONFIGIDFilter != null, e => e.TOCONFIGID <= input.MaxTOCONFIGIDFilter)
                        .WhereIf(input.MinAVAILLIMITFilter != null, e => e.AVAILLIMIT >= input.MinAVAILLIMITFilter)
                        .WhereIf(input.MaxAVAILLIMITFilter != null, e => e.AVAILLIMIT <= input.MaxAVAILLIMITFilter)
                        .WhereIf(input.MinTRANSFERAMOUNTFilter != null, e => e.TRANSFERAMOUNT >= input.MinTRANSFERAMOUNTFilter)
                        .WhereIf(input.MaxTRANSFERAMOUNTFilter != null, e => e.TRANSFERAMOUNT <= input.MaxTRANSFERAMOUNTFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
                        .Where(t => t.TenantId == AbpSession.TenantId && t.TenantId > 0);
            //.WhereIf(!string.IsNullOrWhiteSpace(input.BankBANKNAMEFilter), e => e.BankFk != null && e.BankFk.BANKNAME.ToLower() == input.BankBANKNAMEFilter.ToLower().Trim());

            var pagedAndFilteredBkTransfers = filteredBkTransfers
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var bkTransfers = from o in pagedAndFilteredBkTransfers
                                  //join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                                  //from s1 in j1.DefaultIfEmpty()

                              select new GetBkTransferForViewDto()
                              {
                                  BkTransfer = new BkTransferDto
                                  {
                                      CMPID = o.CMPID,
                                      DOCID = o.DOCID,
                                      DOCDATE = o.DOCDATE,
                                      TRANSFERDATE = o.TRANSFERDATE,
                                      DESCRIPTION = o.DESCRIPTION,
                                      FROMBANKID = o.FROMBANKID,
                                      FROMCONFIGID = o.FROMCONFIGID,
                                      TOBANKID = o.TOBANKID,
                                      TOCONFIGID = o.TOCONFIGID,
                                      AVAILLIMIT = o.AVAILLIMIT,
                                      TRANSFERAMOUNT = o.TRANSFERAMOUNT,
                                      AUDTDATE = o.AUDTDATE,
                                      AUDTUSER = o.AUDTUSER,
                                      STATUS = o.STATUS,
                                      Id = o.Id
                                  },
                                  //BankBANKNAME = o == null ? "" : o.BANKNAME.ToString()
                              };

            var totalCount = await filteredBkTransfers.CountAsync();

            return new PagedResultDto<GetBkTransferForViewDto>(
                totalCount,
                await bkTransfers.ToListAsync()
            );
        }

        public async Task<GetBkTransferForViewDto> GetBkTransferForView(int id)
        {
            var bkTransfer = await _bkTransferRepository.GetAsync(id);

            var output = new GetBkTransferForViewDto { BkTransfer = ObjectMapper.Map<BkTransferDto>(bkTransfer) };

            if (output.BkTransfer.BankId != null)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.BkTransfer.BankId);
                output.BankBANKNAME = _lookupBank.BANKNAME.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BkTransfers_Edit)]
        public async Task<GetBkTransferForEditOutput> GetBkTransferForEdit(EntityDto input)
        {
            var bkTransfer = await _bkTransferRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBkTransferForEditOutput { BkTransfer = ObjectMapper.Map<CreateOrEditBkTransferDto>(bkTransfer) };

            if (output.BkTransfer.FROMBANKID != null)
            {
                var _lookupBank = _lookup_bankRepository.GetAll().Where(b => b.Id == output.BkTransfer.FROMBANKID);
                output.FromBankName = _lookupBank.Select(x => x.BANKNAME).FirstOrDefault();
                output.FromBankAddress = _lookupBank.Select(x => x.ADDR1).FirstOrDefault();
                //output.FromBankAddress = _lookupBank.Select(x => x.ADDR2).FirstOrDefault();
                output.FromBankAccount = _lookupBank.Select(x => x.BKACCTNUMBER).FirstOrDefault().ToString();
            }

            if (output.BkTransfer.TOBANKID != null)
            {
                var _lookupBank = _lookup_bankRepository.GetAll().Where(b => b.Id == output.BkTransfer.TOBANKID);
                output.ToBankName = _lookupBank.Select(x => x.BANKNAME).FirstOrDefault();
                output.ToBankAddress = _lookupBank.Select(x => x.ADDR1).FirstOrDefault();
                //output.ToBankAddress = _lookupBank.Select(x => x.ADDR2).FirstOrDefault();
                output.ToBankAccount = _lookupBank.Select(x => x.BKACCTNUMBER).FirstOrDefault();
            }

            return output;
        }

        public DataForCreateFormDto GetDataForCreateForm()
        {
            var dataForCreateFormDto = new DataForCreateFormDto();
            int docId = 0;
            if (AbpSession.TenantId != null)
            {
                var data = _bkTransferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                if (data.Count() > 0)
                {
                    docId = data.Max(o => o.DOCID);
                    if (docId == 0)
                        docId = 1;
                    else
                        docId = docId + 1;
                    dataForCreateFormDto.DocId = docId;
                }
                else
                {
                    dataForCreateFormDto.DocId = 1;
                }

            }
            return dataForCreateFormDto;
        }

        public async Task CreateOrEdit(CreateOrEditBkTransferDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BkTransfers_Create)]
        protected virtual async Task Create(CreateOrEditBkTransferDto input)
        {
            var bkTransfer = ObjectMapper.Map<BkTransfer>(input);


            if (AbpSession.TenantId != null)
            {
                bkTransfer.TenantId = (int)AbpSession.TenantId;
                var checkUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault();
                if (checkUser != null)
                    bkTransfer.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                input.AUDTUSER = bkTransfer.AUDTUSER;
            }

            if (AbpSession.UserId != null)
            {
                bkTransfer.AUDTUSER = AbpSession.UserId.ToString();
            }
            bkTransfer.AUDTDATE = DateTime.Now;

            bkTransfer.DOCID = GetDataForCreateForm().DocId;


            await _bkTransferRepository.InsertAsync(bkTransfer);
        }

        [AbpAuthorize(AppPermissions.Pages_BkTransfers_Edit)]
        protected virtual async Task Update(CreateOrEditBkTransferDto input)
        {
            input.AUDTDATE = DateTime.Now;
            var bkTransfer = await _bkTransferRepository.FirstOrDefaultAsync((int)input.Id);
            if (AbpSession.TenantId != null)
            {
                bkTransfer.TenantId = (int)AbpSession.TenantId;
                var checkUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault();
                if (checkUser != null)
                    bkTransfer.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                input.AUDTUSER = bkTransfer.AUDTUSER;
            }
            ObjectMapper.Map(input, bkTransfer);
        }

        [AbpAuthorize(AppPermissions.Pages_BkTransfers_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _bkTransferRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBkTransfersToExcel(GetAllBkTransfersForExcelInput input)
        {

            var filteredBkTransfers = _bkTransferRepository.GetAll()
                        //.Include( e => e.BankFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CMPID.Contains(input.Filter) || e.DESCRIPTION.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CMPIDFilter), e => e.CMPID.ToLower() == input.CMPIDFilter.ToLower().Trim())
                        .WhereIf(input.MinDOCIDFilter != null, e => e.DOCID >= input.MinDOCIDFilter)
                        .WhereIf(input.MaxDOCIDFilter != null, e => e.DOCID <= input.MaxDOCIDFilter)
                        .WhereIf(input.MinDOCDATEFilter != null, e => e.DOCDATE >= input.MinDOCDATEFilter)
                        .WhereIf(input.MaxDOCDATEFilter != null, e => e.DOCDATE <= input.MaxDOCDATEFilter)
                        .WhereIf(input.MinTRANSFERDATEFilter != null, e => e.TRANSFERDATE >= input.MinTRANSFERDATEFilter)
                        .WhereIf(input.MaxTRANSFERDATEFilter != null, e => e.TRANSFERDATE <= input.MaxTRANSFERDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DESCRIPTIONFilter), e => e.DESCRIPTION.ToLower() == input.DESCRIPTIONFilter.ToLower().Trim())
                        .WhereIf(input.MinFROMBANKIDFilter != null, e => e.FROMBANKID >= input.MinFROMBANKIDFilter)
                        .WhereIf(input.MaxFROMBANKIDFilter != null, e => e.FROMBANKID <= input.MaxFROMBANKIDFilter)
                        .WhereIf(input.MinFROMCONFIGIDFilter != null, e => e.FROMCONFIGID >= input.MinFROMCONFIGIDFilter)
                        .WhereIf(input.MaxFROMCONFIGIDFilter != null, e => e.FROMCONFIGID <= input.MaxFROMCONFIGIDFilter)
                        .WhereIf(input.MinTOBANKIDFilter != null, e => e.TOBANKID >= input.MinTOBANKIDFilter)
                        .WhereIf(input.MaxTOBANKIDFilter != null, e => e.TOBANKID <= input.MaxTOBANKIDFilter)
                        .WhereIf(input.MinTOCONFIGIDFilter != null, e => e.TOCONFIGID >= input.MinTOCONFIGIDFilter)
                        .WhereIf(input.MaxTOCONFIGIDFilter != null, e => e.TOCONFIGID <= input.MaxTOCONFIGIDFilter)
                        .WhereIf(input.MinAVAILLIMITFilter != null, e => e.AVAILLIMIT >= input.MinAVAILLIMITFilter)
                        .WhereIf(input.MaxAVAILLIMITFilter != null, e => e.AVAILLIMIT <= input.MaxAVAILLIMITFilter)
                        .WhereIf(input.MinTRANSFERAMOUNTFilter != null, e => e.TRANSFERAMOUNT >= input.MinTRANSFERAMOUNTFilter)
                        .WhereIf(input.MaxTRANSFERAMOUNTFilter != null, e => e.TRANSFERAMOUNT <= input.MaxTRANSFERAMOUNTFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim());
            //.WhereIf(!string.IsNullOrWhiteSpace(input.BankBANKNAMEFilter), e => e.BankFk != null && e.BankFk.BANKNAME.ToLower() == input.BankBANKNAMEFilter.ToLower().Trim());

            var query = (from o in filteredBkTransfers
                             //join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                             //from s1 in j1.DefaultIfEmpty()

                         select new GetBkTransferForViewDto()
                         {
                             BkTransfer = new BkTransferDto
                             {
                                 CMPID = o.CMPID,
                                 DOCID = o.DOCID,
                                 DOCDATE = o.DOCDATE,
                                 TRANSFERDATE = o.TRANSFERDATE,
                                 DESCRIPTION = o.DESCRIPTION,
                                 FROMBANKID = o.FROMBANKID,
                                 FROMCONFIGID = o.FROMCONFIGID,
                                 TOBANKID = o.TOBANKID,
                                 TOCONFIGID = o.TOCONFIGID,
                                 AVAILLIMIT = o.AVAILLIMIT,
                                 TRANSFERAMOUNT = o.TRANSFERAMOUNT,
                                 AUDTDATE = o.AUDTDATE,
                                 AUDTUSER = o.AUDTUSER,
                                 Id = o.Id
                             },
                             //BankBANKNAME = s1 == null ? "" : s1.BANKNAME.ToString()
                         });


            var bkTransferListDtos = await query.ToListAsync();

            return _bkTransfersExcelExporter.ExportToFile(bkTransferListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_BkTransfers)]
        public async Task<PagedResultDto<BkTransferBankLookupTableDto>> GetAllBankForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.BANKNAME.ToLower().ToString().Contains(input.Filter.ToLower())
             );
            //IQueryable<Bank> query;
            //if (input.Filter == null)
            //    query = _context.Banks;
            //else
            //    query = _context.Banks.Where(b => b.BANKNAME.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var bankList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BkTransferBankLookupTableDto>();
            foreach (var bank in bankList)
            {
                lookupTableDtoList.Add(new BkTransferBankLookupTableDto
                {
                    Id = bank.Id,
                    DisplayName = bank.BANKNAME?.ToString(),
                    Address = bank.ADDR1 + bank.ADDR2,
                    BankAccount = bank.BKACCTNUMBER,
                    AvailableLimit = Math.Abs(Convert.ToDouble((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                                                join
                                 b in _gltrDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                 on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                                                where (b.AccountID == bank.IDACCTBANK && b.TenantId == AbpSession.TenantId && a.Approved == true)
                                                                select b.Amount).Sum()))
                });
            }

            return new PagedResultDto<BkTransferBankLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public string ProcessBkTransfer(CreateOrEditBkTransferDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var bkTransfer = _bkTransferRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            //Debit Amount
            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = (bkTransfer.TRANSFERAMOUNT * currency.CurrRate),
                AccountID = _lookup_bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == bkTransfer.FROMBANKID).FirstOrDefault().IDACCTBANK,
                Narration = bkTransfer.DESCRIPTION,
                SubAccID = 0
            });

            //Credit Amount
            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = -(bkTransfer.TRANSFERAMOUNT * currency.CurrRate),
                AccountID = _lookup_bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == bkTransfer.TOBANKID).FirstOrDefault().IDACCTBANK,
                Narration = bkTransfer.DESCRIPTION,
                SubAccID = 0
            });

            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = "JV",
                    NARRATION = "BK transfer: " + bkTransfer.DESCRIPTION,
                    DocDate = input.DOCDATE,
                    DocMonth = input.DOCDATE.Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    LocId = 1,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    FmtDocNo = _voucherEntryAppService.GetMaxDocId("JV", true, input.DOCDATE)
                },
                GLTRDetail = gltrdetailsList
            };

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
            {
                var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
                bkTransfer.STATUS = true;
                bkTransfer.GLLINKID = voucher[0].Id;
                bkTransfer.GLDOCID = voucher[0].DocNo;
                var bkTanr = _bkTransferRepository.FirstOrDefault((int)bkTransfer.Id);
                ObjectMapper.Map(bkTransfer, bkTanr);
                alertMsg = "Save";
            }
            else
            {
                alertMsg = "NoRecord";
            }
            return alertMsg;
        }

    }
}