

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.AccountPayables.CRDRNote.Exporting;
using ERP.AccountPayables.CRDRNote.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.Authorization.Users;

namespace ERP.AccountPayables.CRDRNote
{
	[AbpAuthorize(AppPermissions.Pages_CRDRNote)]
    public class CRDRNoteAppService : ERPAppServiceBase, ICRDRNoteAppService
    {
		 private readonly IRepository<CRDRNote> _crDRNoteRepository;
		 private readonly ICRDRNoteExcelExporter _crDRNoteExcelExporter;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<User, long> _userRepository;


        public CRDRNoteAppService(
              IRepository<CRDRNote> crDRNoteRepository, 
              ICRDRNoteExcelExporter crDRNoteExcelExporter,
            IRepository<GLLocation> glLocationRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<User, long> userRepository
        ) 
		  {
			_crDRNoteRepository = crDRNoteRepository;
			_crDRNoteExcelExporter = crDRNoteExcelExporter;
            _glLocationRepository = glLocationRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetCRDRNoteForViewDto>> GetAll(GetAllCRDRNoteInput input)
         {
            var party = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var filteredCRDRNote = _crDRNoteRepository.GetAll().Where(o=>o.TenantId== AbpSession.TenantId && o.TypeID==input.TypeIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter);

            var pagedAndFilteredCRDRNote = filteredCRDRNote
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var crDRNote = from o in pagedAndFilteredCRDRNote
                           join Name in party on o.SubAccID equals Name.Id into Name1
                           from Names in Name1.DefaultIfEmpty() 
                         select new GetCRDRNoteForViewDto() {
							CRDRNote = new CRDRNoteDto
							{
                                LocID=o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                PostingDate = o.PostingDate,
                                PaymentDate = o.PaymentDate,
                                TypeID = o.TypeID,
                                TransType = o.TransType,
                                AccountID = o.AccountID,
                                SubAccID = o.SubAccID,
                                InvoiceNo = o.InvoiceNo,
                                PartyInvNo = o.PartyInvNo,
                                PartyInvDate = o.PartyInvDate,
                                PartyInvAmount = o.PartyInvAmount,
                                Amount = o.Amount,
                                Reason = o.Reason,
                                StkAccID=o.StkAccID,
                                Posted = o.Posted,
                                LinkDetID = o.LinkDetID,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id,
                                PartyName = Names.SubAccName
							}
						};

            var totalCount = await filteredCRDRNote.CountAsync();

            return new PagedResultDto<GetCRDRNoteForViewDto>(
                totalCount,
                await crDRNote.ToListAsync()
            );
         }

        public async Task<GetCRDRNoteForViewDto> GetCRDRNoteForView(int id)
        {
            var crDRNote = await _crDRNoteRepository.GetAsync(id);

            var output = new GetCRDRNoteForViewDto { CRDRNote = ObjectMapper.Map<CRDRNoteDto>(crDRNote) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CRDRNote_Edit)]
		 public async Task<GetCRDRNoteForEditOutput> GetCRDRNoteForEdit(EntityDto input)
         {
            var CRDRNote = await _crDRNoteRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCRDRNoteForEditOutput {CRDRNote = ObjectMapper.Map<CreateOrEditCRDRNoteDto>(CRDRNote)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCRDRNoteDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CRDRNote_Create)]
		 protected virtual async Task Create(CreateOrEditCRDRNoteDto input)
         {
            var crdrNote = ObjectMapper.Map<CRDRNote>(input);
            crdrNote.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            crdrNote.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            crdrNote.CreateDate = DateTime.Now;

            if (AbpSession.TenantId != null)
			{
                crdrNote.TenantId = (int) AbpSession.TenantId;
			}

            crdrNote.DocNo = GetMaxID(Convert.ToInt32(input.TypeID));
            await _crDRNoteRepository.InsertAsync(crdrNote);
         }

		 [AbpAuthorize(AppPermissions.Pages_CRDRNote_Edit)]
		 protected virtual async Task Update(CreateOrEditCRDRNoteDto input)
         {
            var CRDRNote = await _crDRNoteRepository.FirstOrDefaultAsync((int)input.Id);
            input.CreateDate = CRDRNote.CreateDate;
             ObjectMapper.Map(input, CRDRNote);
        }

		 [AbpAuthorize(AppPermissions.Pages_CRDRNote_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _crDRNoteRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCRDRNoteToExcel(GetAllCRDRNoteForExcelInput input)
         {
			
			var filteredCRDRNote = _crDRNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == input.TypeIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter);

            var query = (from o in filteredCRDRNote
                         select new GetCRDRNoteForViewDto() { 
							CRDRNote = new CRDRNoteDto
							{
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                PostingDate = o.PostingDate,
                                PaymentDate = o.PaymentDate,
                                TypeID = o.TypeID,
                                TransType = o.TransType,
                                AccountID = o.AccountID,
                                SubAccID = o.SubAccID,
                                InvoiceNo = o.InvoiceNo,
                                PartyInvNo = o.PartyInvNo,
                                PartyInvDate = o.PartyInvDate,
                                PartyInvAmount = o.PartyInvAmount,
                                Amount = o.Amount,
                                Reason = o.Reason,
                                StkAccID = o.StkAccID,
                                Posted = o.Posted,
                                LinkDetID = o.LinkDetID,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id

                            }
						 });


            var CRDRNoteListDtos = await query.ToListAsync();

            return _crDRNoteExcelExporter.ExportToFile(CRDRNoteListDtos);
         }

        public int GetMaxID(int typeID)
        {
            int maxID = ((from tab1 in _crDRNoteRepository.GetAll().Where(o => o.TypeID==typeID && o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            return maxID;
        }

        public string GetLocationName(int locID)
        {

            string locName = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).Count() > 0 ?
                                       _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).SingleOrDefault().LocDesc : "";
            return locName;

        }

        public string GetChartOfAccountName(string accID, string type)
        {
            string accName;

            switch (type)
            {
                case "AccountID":
                  accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                       _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
                    break;
                case "StkAccID":
                    accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                         _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
                    break;
                default:
                    accName = null;
                    break;
            }
            return accName;

        }
        public string GetSubLedgerName(int subAccID,string accID)
        {

            string subName = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == subAccID && o.AccountID==accID).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == subAccID && o.AccountID == accID).SingleOrDefault().SubAccName : "";
            return subName;

        }



    }

   
}